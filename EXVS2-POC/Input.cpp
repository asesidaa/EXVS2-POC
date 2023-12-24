#include <Windows.h>
#include <Hidclass.h>
#include <Hidusage.h>
#include <hidpi.h>
#include <sal.h>
#include <string.h>

#include <array>
#include <atomic>
#include <memory>
#include <mutex>
#include <optional>
#include <string>
#include <unordered_map>
#include <utility>

#include "Configs.h"
#include "Input.h"
#include "log.h"

#pragma comment(lib, "hid.lib")

static const char* HidP_strerror(NTSTATUS rc)
{
    switch (rc)
    {
    case HIDP_STATUS_SUCCESS:
        return "HIDP_STATUS_SUCCESS";
    case HIDP_STATUS_INVALID_REPORT_LENGTH:
        return "HIDP_INVALID_REPORT_LENGTH";
    case HIDP_STATUS_INVALID_REPORT_TYPE:
        return "HIDP_INVALID_REPORT_TYPE";
    case HIDP_STATUS_BUFFER_TOO_SMALL:
        return "HIDP_STATUS_BUFFER_TOO_SMALL";
    case HIDP_STATUS_INCOMPATIBLE_REPORT_ID:
        return "HIDP_STATUS_INCOMPATIBLE_REPORT_ID";
    case HIDP_STATUS_INVALID_PREPARSED_DATA:
        return "HIDP_STATUS_INVALID_PREPARSED_DATA";
    case HIDP_STATUS_USAGE_NOT_FOUND:
        return "HIDP_STATUS_USAGE_NOT_FOUND";
    default:
        return "<unknown>";
    }
}

static void PressUp(InputState* out)
{
    out->Y = Direction::Negative;
}

static void PressDown(InputState* out)
{
    out->Y = Direction::Positive;
}

static void PressLeft(InputState* out)
{
    out->X = Direction::Negative;
}

static void PressRight(InputState* out)
{
    out->X = Direction::Positive;
}

#define PRES_BUTAN(name) static void Press ## name(InputState* out) { out->name = true; }
PRES_BUTAN(A)
PRES_BUTAN(B)
PRES_BUTAN(C)
PRES_BUTAN(D)
PRES_BUTAN(Start)
PRES_BUTAN(Coin)
PRES_BUTAN(Card)
PRES_BUTAN(Test)
PRES_BUTAN(Service)
PRES_BUTAN(Kill)
#undef PRES_BUTAN

static void InputStateGetKeyboard(InputState* out, InputConfig* config)
{
#define KEYBIND(name, kb_default, dinput_default)                                                                      \
    for (int key : config->KeyboardBindings.name)                                                                      \
    {                                                                                                                  \
        if (GetAsyncKeyState(key) & 0x8000)                                                                            \
        {                                                                                                              \
            (Press##name)(out);                                                                                        \
            break;                                                                                                     \
        }                                                                                                              \
    }

    KEYBINDS()
#undef KEYBIND
}

static std::optional<std::string> GetRawInputDevicePath(HANDLE handle)
{
    std::string devicePath(4096, '\0');
    unsigned int len = static_cast<unsigned int>(devicePath.size());
    unsigned int rc = GetRawInputDeviceInfoA(handle, RIDI_DEVICENAME, devicePath.data(), &len);
    if (rc == -1 || rc == 0)
    {
        err("failed to get RawInput device info");
        return {};
    }

    // rc includes the null terminator.
    devicePath.resize(rc - 1);
    return devicePath;
}

struct InputDevice
{
  private:
    InputDevice(HANDLE handle, std::string&& devicePath, _HIDP_PREPARSED_DATA* preparsedData)
        : handle_(handle), path_(std::move(devicePath)), preparsedData_(preparsedData)
    {
        ParseCaps();
    }

  public:
    ~InputDevice()
    {
        free(preparsedData_);
    }

    InputDevice(const InputDevice& copy) = delete;

    InputDevice(InputDevice&& move)
        : handle_(move.handle_), path_(std::move(move.path_)), preparsedData_(move.preparsedData_)
    {
        move.handle_ = nullptr;
        move.preparsedData_ = nullptr;
    }

    InputDevice& operator=(const InputDevice& copy) = delete;
    InputDevice& operator=(InputDevice&& move)
    {
        if (&move == this)
            return *this;
        if (preparsedData_)
        {
            free(preparsedData_);
            preparsedData_ = nullptr;
        }

        this->handle_ = move.handle_;
        this->path_ = std::move(move.path_);
        this->preparsedData_ = move.preparsedData_;

        move.handle_ = nullptr;
        move.preparsedData_ = nullptr;
    }

  private:
    void GetCapRange(unsigned long* min, unsigned long* max, const HIDP_VALUE_CAPS& cap)
    {
        *min = cap.LogicalMin;
        if (cap.LogicalMax == -1)
        {
            *max = (1UL << cap.BitSize) - 1;
        }
        else
        {
            *max = cap.LogicalMax;
        }
    }

    void ParseCaps()
    {
        HIDP_CAPS caps;
        if (HidP_GetCaps(preparsedData_, &caps) != HIDP_STATUS_SUCCESS)
            fatal("HidP_GetCaps failed");

        std::vector<HIDP_VALUE_CAPS> valueCaps;
        unsigned short len = caps.NumberInputValueCaps;
        valueCaps.resize(len);
        if (HidP_GetValueCaps(HidP_Input, valueCaps.data(), &len, preparsedData_) != HIDP_STATUS_SUCCESS)
            fatal("HidP_GetValueCaps failed");

        for (const HIDP_VALUE_CAPS& cap : valueCaps)
        {
            if (cap.IsRange)
            {
                warn("skipping unhandled value range");
                continue;
            }
            switch (cap.NotRange.Usage)
            {
            case HID_USAGE_GENERIC_X:
                GetCapRange(&xMin_, &xMax_, cap);
                break;

            case HID_USAGE_GENERIC_Y:
                GetCapRange(&yMin_, &yMax_, cap);
                break;

            case HID_USAGE_GENERIC_HATSWITCH:
                GetCapRange(&dpadMin_, &dpadMax_, cap);
                if (dpadMax_ - dpadMin_ != 7)
                {
                    err("unexpected dpad range: min = %d, max = %d", dpadMin_, dpadMax_);
                    dpadMin_ = 0;
                    dpadMax_ = 0;
                }
                break;

            default:
                break;
            }
        }
    }

  public:
    static std::unique_ptr<InputDevice> FromHandle(HANDLE handle)
    {
        std::optional<std::string> devicePath = GetRawInputDevicePath(handle);
        if (!devicePath)
            return {};

        unsigned int len = 0;
        GetRawInputDeviceInfoA(handle, RIDI_PREPARSEDDATA, nullptr, &len);

        auto* preparsedData = static_cast<_HIDP_PREPARSED_DATA*>(malloc(len));
        unsigned int rc = GetRawInputDeviceInfoA(handle, RIDI_PREPARSEDDATA, preparsedData, &len);
        if (rc != len)
            fatal("GetRawInputDeviceInfoA returned different length for RIDI_PREPAREDDATA?");

        return std::unique_ptr<InputDevice>(new InputDevice(handle, std::move(*devicePath), preparsedData));
    }

    HANDLE Handle() const
    {
        return handle_;
    }

    const std::string& Path() const
    {
        return path_;
    }

  private:
    void ParseButtons(InputState* result, InputConfig* config, char* hidReport, unsigned long hidReportLen)
    {
        std::array<USAGE, 64> usages;
        unsigned long len = static_cast<unsigned long>(usages.size());
        NTSTATUS rc = HidP_GetUsages(HidP_Input, HID_USAGE_PAGE_BUTTON, 0, usages.data(), &len, preparsedData_,
                                     hidReport, hidReportLen);
        if (rc != HIDP_STATUS_SUCCESS)
        {
            err("HidP_GetUsages failed: %s", HidP_strerror(rc));
            return;
        }

        unsigned int buttonMask = 0;
        for (size_t i = 0; i < len; ++i)
        {
            buttonMask |= 1u << (usages[i] - 1);
        }

#define KEYBIND(name, kb_default, dinput_default)                                                                      \
    {                                                                                                                  \
        unsigned int bindMask = 0;                                                                                     \
        for (int key : config->ControllerBindings.name)                                                                \
            bindMask |= 1u << (key - 1);                                                                               \
        if (buttonMask & bindMask)                                                                                     \
            (Press##name)(result);                                                                                     \
    }
        KEYBINDS()
    }

    double Scale(unsigned long value, unsigned long min, unsigned long max)
    {
        double range = max - min;
        return (static_cast<double>(value) - min) / range;
    }

    void ParseDirection(InputState* result, char* hidReport, unsigned long hidReportLen)
    {
        // Our coordinate system is the opposite of HID's.
        if (xMin_ != xMax_ && yMin_ != yMax_)
        {
            unsigned long xValue = 0;
            if (HidP_GetUsageValue(HidP_Input, HID_USAGE_PAGE_GENERIC, 0, HID_USAGE_GENERIC_X, &xValue, preparsedData_,
                                   hidReport, hidReportLen) == HIDP_STATUS_SUCCESS)
            {
                double x = Scale(xValue, xMin_, xMax_);
                if (x < 0.25)
                    result->X = Direction::Negative;
                else if (x > 0.75)
                    result->X = Direction::Positive;
            }

            unsigned long yValue = 0;
            if (HidP_GetUsageValue(HidP_Input, HID_USAGE_PAGE_GENERIC, 0, HID_USAGE_GENERIC_Y, &yValue, preparsedData_,
                                   hidReport, hidReportLen) == HIDP_STATUS_SUCCESS)
            {
                double y = Scale(yValue, yMin_, yMax_);
                if (y < 0.25)
                    result->Y = Direction::Negative;
                else if (y > 0.75)
                    result->Y = Direction::Positive;
            }
        }

        if (dpadMin_ != dpadMax_)
        {
            unsigned long dpadValue = 0;
            if (HidP_GetUsageValue(HidP_Input, HID_USAGE_PAGE_GENERIC, 0, HID_USAGE_GENERIC_HATSWITCH, &dpadValue,
                                   preparsedData_, hidReport, hidReportLen) == HIDP_STATUS_SUCCESS)
            {
                unsigned long diff = dpadValue - dpadMin_;
                // Clockwise starting with north
                switch (diff)
                {
                case 0:
                    result->X = Direction::Neutral;
                    result->Y = Direction::Negative;
                    break;

                case 1:
                    result->X = Direction::Positive;
                    result->Y = Direction::Negative;
                    break;

                case 2:
                    result->X = Direction::Positive;
                    result->Y = Direction::Neutral;
                    break;

                case 3:
                    result->X = Direction::Positive;
                    result->Y = Direction::Positive;
                    break;

                case 4:
                    result->X = Direction::Neutral;
                    result->Y = Direction::Positive;
                    break;

                case 5:
                    result->X = Direction::Negative;
                    result->Y = Direction::Positive;
                    break;

                case 6:
                    result->X = Direction::Negative;
                    result->Y = Direction::Neutral;
                    break;

                case 7:
                    result->X = Direction::Negative;
                    result->Y = Direction::Negative;
                    break;

                default:
                    break;
                }
            }
        }
    }

  public:
    InputState Parse(InputConfig* config, RAWINPUT* input)
    {
        InputState result;

        char* p = reinterpret_cast<char*>(input->data.hid.bRawData +
                                          (input->data.hid.dwCount - 1) * input->data.hid.dwSizeHid);

        ParseButtons(&result, config, p, input->data.hid.dwSizeHid);
        ParseDirection(&result, p, input->data.hid.dwSizeHid);

        return result;
    }

  private:
    HANDLE handle_ = nullptr;
    std::string path_;
    _HIDP_PREPARSED_DATA* preparsedData_ = nullptr;

    unsigned long dpadMin_ = 0;
    unsigned long dpadMax_ = 0;

    unsigned long xMin_ = 0;
    unsigned long xMax_ = 0;

    unsigned long yMin_ = 0;
    unsigned long yMax_ = 0;
};

struct RawInputManager
{
    InputState GetInputState()
    {
        InputState result;
        std::shared_ptr<InputConfig> config = inputConfig_.load();
        std::shared_ptr<InputState> controllerState = lastControllerState_;

        if (config->KeyboardEnabled)
            InputStateGetKeyboard(&result, config.get());

        if (config->ControllerEnabled && controllerState)
            result.Merge(*controllerState);

        return result;
    }

    void DeviceArrived(HANDLE handle)
    {
        std::lock_guard<std::mutex> lock(mutex_);

        std::shared_ptr<InputDevice> device = InputDevice::FromHandle(handle);
        if (!device)
            return;

        info("Device arrived: %s", device->Path().c_str());

        devices_.insert({handle, device});
        devicesByPath_.insert({device->Path(), device});
        for (size_t i = 0; i < devicesByIndex_.size(); ++i)
        {
            if (!devicesByIndex_[i])
            {
                info("Assigning device to index %zu", i);
                devicesByIndex_[i] = device;
                break;
            }
        }

        UpdateCurrentDeviceLocked();
    }

    void DeviceInput(HRAWINPUT hRawInput)
    {
        unsigned int len = 0;
        GetRawInputData(hRawInput, RID_INPUT, nullptr, &len, sizeof(RAWINPUTHEADER));
        RAWINPUT* rawinput = static_cast<RAWINPUT*>(malloc(len));
        unsigned int rc = GetRawInputData(hRawInput, RID_INPUT, rawinput, &len, sizeof(RAWINPUTHEADER));
        if (rc != len)
            fatal("GetRawInputData returned different length?");

        std::shared_ptr<InputDevice> device;
        {
            std::lock_guard<std::mutex> lock(mutex_);
            device = currentDevice_;
        }

        if (!device || device->Handle() != rawinput->header.hDevice)
        {
            free(rawinput);
            return;
        }

        if (rawinput->header.dwType != RIM_TYPEHID)
        {
            err("got a RawInput header that wasn't HID?");
            free(rawinput);
            return;
        }

        std::shared_ptr<InputConfig> config = inputConfig_.load();
        InputState newState = device->Parse(config.get(), rawinput);
        lastControllerState_.store(std::make_shared<InputState>(newState));
        free(rawinput);
    }

    void DeviceLeft(HANDLE handle)
    {
        std::lock_guard<std::mutex> lock(mutex_);

        auto it = devices_.find(handle);
        if (it == devices_.end())
            fatal("state corruption: failed to find device that left");

        for (auto& device : devicesByIndex_)
        {
            if (device == it->second)
            {
                device = nullptr;
            }
        }

        devicesByPath_.erase(it->second->Path());
        devices_.erase(it);

        UpdateCurrentDeviceLocked();
    }

    void UpdateConfig(std::shared_ptr<InputConfig> newConfig)
    {
        std::lock_guard<std::mutex> lock(mutex_);
        inputConfig_.store(newConfig);
        UpdateCurrentDeviceLocked();
    }

  private:
    _Requires_lock_held_(mutex_) void UpdateCurrentDeviceLocked()
    {
        std::shared_ptr<InputConfig> config = inputConfig_.load();

        std::shared_ptr<InputDevice> oldDevice;
        std::swap(oldDevice, currentDevice_);

        if (config->ControllerPath)
        {
            auto it = devicesByPath_.find(*config->ControllerPath);
            if (it != devicesByPath_.end())
            {
                currentDevice_ = it->second;
            }
        }
        else
        {
            int controllerId = config->ControllerDeviceId;
            if (controllerId == 16)
            {
                // Find the first set device.
                for (const auto& device : devicesByIndex_)
                {
                    if (device != nullptr)
                    {
                        currentDevice_ = device;
                        break;
                    }
                }
            }
            else if (controllerId >= 0 && controllerId < 16)
            {
                currentDevice_ = devicesByIndex_[controllerId];
            }
        }

        if (oldDevice != currentDevice_)
        {
            if (currentDevice_)
            {
                info("Changed current input device: %p %s", currentDevice_->Handle(), currentDevice_->Path().c_str());
            }
            else
            {
                info("Unassigned current input device");
            }
        }
    }

    std::atomic<std::shared_ptr<InputConfig>> inputConfig_ = nullptr;
    std::atomic<std::shared_ptr<InputState>> lastControllerState_ = nullptr;

    std::mutex mutex_;
    _Guarded_by_(mutex_) std::shared_ptr<InputDevice> currentDevice_ = nullptr;

    _Guarded_by_(mutex_) std::unordered_map<HANDLE, std::shared_ptr<InputDevice>> devices_;
    _Guarded_by_(mutex_) std::unordered_map<std::string, std::shared_ptr<InputDevice>> devicesByPath_;
    _Guarded_by_(mutex_) std::array<std::shared_ptr<InputDevice>, 16> devicesByIndex_;
};

static RawInputManager g_inputManager;

void UpdateInputConfig(InputConfig&& newConfig)
{
    auto config = std::make_shared<InputConfig>(std::move(newConfig));
    debug("Updating input config:\n%s", config->Dump("  ").c_str());
    g_inputManager.UpdateConfig(std::move(config));
}

InputState InputState::Get()
{
    return g_inputManager.GetInputState();
}

static LRESULT InputWindowProc(HWND hwnd, UINT msg, WPARAM wparam, LPARAM lparam)
{
    if (msg == WM_INPUT_DEVICE_CHANGE)
    {
        info("Received WM_INPUT_DEVICE_CHANGE");
        if (wparam == GIDC_ARRIVAL)
        {
            g_inputManager.DeviceArrived(reinterpret_cast<HANDLE>(lparam));
        }
        else if (wparam == GIDC_REMOVAL)
        {
            g_inputManager.DeviceLeft(reinterpret_cast<HANDLE>(lparam));
        }
    }
    else if (msg == WM_INPUT)
    {
        g_inputManager.DeviceInput(reinterpret_cast<HRAWINPUT>(lparam));
    }
    else
    {
        return DefWindowProc(hwnd, msg, wparam, lparam);
    }

    return 0;
}

void InitializeInput()
{
    HINSTANCE hInstance = GetModuleHandle(nullptr);
    WNDCLASSA wndClass = {
        .style = 0,
        .lpfnWndProc = InputWindowProc,
        .cbClsExtra = 0,
        .cbWndExtra = 0,
        .hInstance = hInstance,
        .hIcon = nullptr,
        .hCursor = nullptr,
        .hbrBackground = nullptr,
        .lpszMenuName = nullptr,
        .lpszClassName = "POCRawInput",
    };

    ATOM atom = RegisterClassA(&wndClass);
    if (atom == 0)
        fatal("failed to register Raw Input window class");

    HWND messageWindow = CreateWindowA("POCRawInput", nullptr, 0, 0, 0, 0, 0, HWND_MESSAGE, nullptr, hInstance, 0);
    if (messageWindow == INVALID_HANDLE_VALUE)
        fatal("failed to create message window");
    SendMessage(messageWindow, WM_USER, 420, 69);

    RAWINPUTDEVICE devices[2] = {
        {
            .usUsagePage = HID_USAGE_PAGE_GENERIC,
            .usUsage = HID_USAGE_GENERIC_JOYSTICK,
            .dwFlags = RIDEV_DEVNOTIFY | RIDEV_INPUTSINK,
            .hwndTarget = messageWindow,
        },
        {
            .usUsagePage = HID_USAGE_PAGE_GENERIC,
            .usUsage = HID_USAGE_GENERIC_GAMEPAD,
            .dwFlags = RIDEV_DEVNOTIFY | RIDEV_INPUTSINK,
            .hwndTarget = messageWindow,
        },
    };
    if (!RegisterRawInputDevices(devices, 2, sizeof(RAWINPUTDEVICE)))
        fatal("failed to register for Raw Input devices");
}
