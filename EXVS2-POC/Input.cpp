#define _SILENCE_CXX20_OLD_SHARED_PTR_ATOMIC_SUPPORT_DEPRECATION_WARNING
#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING

// ReSharper disable CppClangTidyClangDiagnosticUnusedMacros
// ReSharper disable CppClangTidyClangDiagnosticMicrosoftCast

#include <codecvt>

#include <Windows.h>
#include <Hidclass.h>
#include <Hidusage.h>
#include <hidpi.h>
#include <hidsdi.h>
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

#include "minhook.h"

#include "Configs.h"
#include "Input.h"
#include "log.h"
#include "util.h"

#pragma comment(lib, "hid.lib")

static const std::string XINPUT_NATIVE = "xinput-native";
static const std::string XINPUT = "xinput";

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
        name_ = FetchName();
    }

  public:
    ~InputDevice()
    {
        free(preparsedData_);
    }

    InputDevice(const InputDevice& copy) = delete;

    InputDevice(InputDevice&& move)
        : handle_(move.handle_), path_(std::move(move.path_)), name_(std::move(move.name_)), preparsedData_(move.preparsedData_)
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
        this->name_ = std::move(move.name_);
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

    std::optional<std::string> FetchName()
    {
        HANDLE hidHandle = CreateFileA(path_.c_str(), 0, FILE_SHARE_READ | FILE_SHARE_WRITE, nullptr, OPEN_EXISTING, 0, nullptr);
        std::wstring buf;

        buf.resize(127);
        if (!HidD_GetProductString(hidHandle, buf.data(), static_cast<ULONG>(buf.size())))
        {
            warn("failed to get rawinput device name");
            CloseHandle(hidHandle);
            return {};
        }

        buf.resize(wcslen(buf.c_str()));
        std::string result = std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>, wchar_t>{}.to_bytes(buf);

        CloseHandle(hidHandle);
        return result;
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

    const std::optional<std::string>& Name() const
    {
        return name_;
    }

  private:
    unsigned int ReadButtons(char* hidReport, unsigned long hidReportLen)
    {
        std::array<USAGE, 64> usages;
        unsigned long len = static_cast<unsigned long>(usages.size());
        NTSTATUS rc = HidP_GetUsages(HidP_Input, HID_USAGE_PAGE_BUTTON, 0, usages.data(), &len, preparsedData_,
                                     hidReport, hidReportLen);
        if (rc != HIDP_STATUS_SUCCESS)
        {
            err("HidP_GetUsages failed: %s", HidP_strerror(rc));
            return 0;
        }

        unsigned int buttonMask = 0;
        for (size_t i = 0; i < len; ++i)
        {
            buttonMask |= 1u << (usages[i] - 1);
        }

        return buttonMask;
    }

    void ParseButtons(InputState* result, const KeyBinds* config, char* hidReport, unsigned long hidReportLen)
    {
    }

    double Scale(unsigned long value, unsigned long min, unsigned long max)
    {
        double range = max - min;
        return (static_cast<double>(value) - min) / range;
    }

    std::pair<Direction, Direction> ReadDPad(char* hidReport, unsigned long hidReportLen)
    {
        std::pair<Direction, Direction> result;
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
                    result.first = Direction::Neutral;
                    result.second = Direction::Negative;
                    break;

                case 1:
                    result.first = Direction::Positive;
                    result.second = Direction::Negative;
                    break;

                case 2:
                    result.first = Direction::Positive;
                    result.second = Direction::Neutral;
                    break;

                case 3:
                    result.first = Direction::Positive;
                    result.second = Direction::Positive;
                    break;

                case 4:
                    result.first = Direction::Neutral;
                    result.second = Direction::Positive;
                    break;

                case 5:
                    result.first = Direction::Negative;
                    result.second = Direction::Positive;
                    break;

                case 6:
                    result.first = Direction::Negative;
                    result.second = Direction::Neutral;
                    break;

                case 7:
                    result.first = Direction::Negative;
                    result.second = Direction::Negative;
                    break;

                default:
                    break;
                }
            }
        }
        return result;
    }

    std::pair<double, double> ReadLStick(char* hidReport, unsigned long hidReportLen)
    {
        std::pair<double, double> result = { 0.5, 0.5 };
        if (xMin_ != xMax_ && yMin_ != yMax_)
        {
            unsigned long xValue = 0;
            if (HidP_GetUsageValue(HidP_Input, HID_USAGE_PAGE_GENERIC, 0, HID_USAGE_GENERIC_X, &xValue, preparsedData_,
                                   hidReport, hidReportLen) == HIDP_STATUS_SUCCESS)
            {
                result.first = Scale(xValue, xMin_, xMax_);
            }

            unsigned long yValue = 0;
            if (HidP_GetUsageValue(HidP_Input, HID_USAGE_PAGE_GENERIC, 0, HID_USAGE_GENERIC_Y, &yValue, preparsedData_,
                                   hidReport, hidReportLen) == HIDP_STATUS_SUCCESS)
            {
                result.second = Scale(yValue, yMin_, yMax_);
            }
        }

        return result;
    }

    std::pair<Direction, Direction> ReadDirection(char* hidReport, unsigned long hidReportLen)
    {
        std::pair<Direction, Direction> dpad = ReadDPad(hidReport, hidReportLen);
        std::pair<double, double> stick = ReadLStick(hidReport, hidReportLen);

        std::pair<Direction, Direction> result;
        double x = stick.first;
        double y = stick.second;
        if (x < 0.25)
            result.first = Direction::Negative;
        else if (x > 0.75)
            result.first = Direction::Positive;

        if (y < 0.25)
            result.second = Direction::Negative;
        else if (y > 0.75)
            result.second = Direction::Positive;

        if (dpad.first != Direction::Neutral)
            result.first = dpad.first;

        if (dpad.second != Direction::Neutral)
            result.second = dpad.second;

        return result;
    }


    void ParseDirection(InputState* result, char* hidReport, unsigned long hidReportLen)
    {
        std::pair<Direction, Direction> direction = ReadDirection(hidReport, hidReportLen);
        result->X = direction.first;
        result->Y = direction.second;
    }

  public:
    InputState Parse(const KeyBinds* config, RAWINPUT* input)
    {
        InputState result = {};

        unsigned long hidReportLen = input->data.hid.dwSizeHid;
        char* hidReport = reinterpret_cast<char*>(
            input->data.hid.bRawData + (input->data.hid.dwCount - 1) * hidReportLen
        );

        unsigned int buttonMask = ReadButtons(hidReport, hidReportLen);
#define KEYBIND(name, kb_default, dinput_default)                                                                      \
        {                                                                                                              \
            unsigned int bindMask = 0;                                                                                 \
            for (int key : config->name)                                                                               \
                bindMask |= 1u << (key - 1);                                                                           \
            if (buttonMask & bindMask)                                                                                 \
                (Press##name)(&result);                                                                                \
        }
        KEYBINDS()
#undef KEYBIND

        std::pair<Direction, Direction> direction = ReadDirection(hidReport, hidReportLen);
        result.X = direction.first;
        result.Y = direction.second;

        return result;
    }

    XINPUT_GAMEPAD ParseXInput(const XInputKeyBinds* config, RAWINPUT* input)
    {
        XINPUT_GAMEPAD result = {};

        unsigned long hidReportLen = input->data.hid.dwSizeHid;
        char* hidReport = reinterpret_cast<char*>(
            input->data.hid.bRawData + (input->data.hid.dwCount - 1) * hidReportLen
        );

        unsigned int buttonMask = ReadButtons(hidReport, hidReportLen);
#define XINPUT_KEYBIND(name, xinput_button, xinput_trigger, default_bind)                       \
        {                                                                                       \
            unsigned int bindMask = 0;                                                          \
            for (int key : config->name)                                                        \
                bindMask |= 1u << (key - 1);                                                    \
            if (buttonMask & bindMask)                                                          \
            {                                                                                   \
                if (xinput_button != 0)                                                         \
                    result.wButtons |= xinput_button;                                           \
                if (xinput_trigger != 0)                                                        \
                    (xinput_trigger == -1 ? result.bLeftTrigger : result.bRightTrigger) = 255;  \
            }                                                                                   \
        }
        XINPUT_KEYBINDS()
#undef XINPUT_KEYBIND

        std::pair<Direction, Direction> direction = ReadDPad(hidReport, hidReportLen);
        switch (direction.first)
        {
            case Direction::Negative:
                result.wButtons |= XINPUT_GAMEPAD_DPAD_LEFT;
                break;
            case Direction::Positive:
                result.wButtons |= XINPUT_GAMEPAD_DPAD_RIGHT;
                break;
            default:
                break;
        }
        switch (direction.second)
        {
            case Direction::Negative:
                result.wButtons |= XINPUT_GAMEPAD_DPAD_UP;
                break;
            case Direction::Positive:
                result.wButtons |= XINPUT_GAMEPAD_DPAD_DOWN;
                break;
            default:
                break;
        }

        std::pair<double, double> analog = ReadLStick(hidReport, hidReportLen);
        result.sThumbLX = static_cast<short>((analog.first * 2 - 1.0) * 32767);
        result.sThumbLY = static_cast<short>((1.0 - analog.second * 2) * 32767);

        return result;
    }

  private:
    HANDLE handle_ = nullptr;
    std::string path_;
    std::optional<std::string> name_;
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
    bool HasNativeXInputController()
    {
        std::shared_ptr<InputConfig> config = inputConfig_.load();

        if (!config->EmulatedXInputEnabled)
        {
            return true;
        }
        
        for (const auto& it : config->Controllers)
        {
            const auto& controller = it.second;

            if (controller.Mode.value() == XINPUT_NATIVE && controller.LastXInputState)
            {
                return true;
            }
        }

        return false;
    }
    
    InputState GetInputState()
    {
        InputState result;
        std::shared_ptr<InputConfig> config = inputConfig_.load();

        if (config->KeyboardEnabled)
            InputStateGetKeyboard(&result, config.get());

        for (const auto& it : config->Controllers)
        {
            const auto& controller = it.second;
            std::shared_ptr<InputState> state = std::atomic_load(&controller.LastState);
            if (controller.Enabled && state)
            {
                result.Merge(*state);
            }
        }

        return result;
    }

    bool HasXInputController()
    {
        std::shared_ptr<InputConfig> config = inputConfig_.load();

        if (!config->EmulatedXInputEnabled)
        {
            return false;
        }
        
        for (const auto& it : config->Controllers)
        {
            const auto& controller = it.second;

            if (controller.Mode.value() == XINPUT_NATIVE)
            {
                continue;
            }
            
            if (std::get_if<XInputKeyBinds>(&controller.Bindings))
                return true;
        }

        return false;
    }


    std::optional<XINPUT_STATE> GetXInputState()
    {
        std::shared_ptr<InputConfig> config = inputConfig_.load();
        for (const auto& it : config->Controllers)
        {
            const auto& controller = it.second;

            if (controller.Mode.value() == XINPUT_NATIVE)
            {
                continue;
            }
            
            auto xinputState = std::atomic_load(&controller.LastXInputState);
            if (controller.Enabled && xinputState)
                return *xinputState;
        }

        return {};
    }

    void DeviceArrived(HANDLE handle)
    {
        std::shared_ptr<InputDevice> device = InputDevice::FromHandle(handle);
        if (!device)
            return;

        info("Device arrived: %s", device->Path().c_str());

        std::lock_guard<std::mutex> lock(mutex_);
        devices_.insert({handle, device});
        for (size_t i = 0; i < devicesByIndex_.size(); ++i)
        {
            if (!devicesByIndex_[i])
            {
                info("Assigning device to index %zu", i);
                devicesByIndex_[i] = device;
                break;
            }
        }

        DumpDevicesLocked();
    }

    void DeviceInput(HRAWINPUT hRawInput)
    {
        unsigned int len = 0;
        GetRawInputData(hRawInput, RID_INPUT, nullptr, &len, sizeof(RAWINPUTHEADER));
        std::unique_ptr<char[]> rawinput_buf = std::make_unique<char[]>(len);
        RAWINPUT* rawinput = reinterpret_cast<RAWINPUT*>(rawinput_buf.get());
        unsigned int rc = GetRawInputData(hRawInput, RID_INPUT, rawinput, &len, sizeof(RAWINPUTHEADER));
        if (rc != len)
            fatal("GetRawInputData returned different length?");

        if (rawinput->header.dwType != RIM_TYPEHID)
        {
            err("got a RawInput header that wasn't HID?");
            return;
        }

        std::shared_ptr<InputDevice> device;

        {
            std::lock_guard<std::mutex> lock(mutex_);

            {
                auto it = devices_.find(rawinput->header.hDevice);
                if (it == devices_.end()) return;
                device = it->second;
            }
        }

        if (!device) return;

        std::shared_ptr<InputConfig> config = inputConfig_.load();
        for (auto& it : config->Controllers)
        {
            ControllerConfig& controllerConfig = it.second;
            if (!controllerConfig.Enabled) continue;

            bool matched = false;
            matched |= controllerConfig.DevicePath && controllerConfig.DevicePath == device->Path();
            matched |= controllerConfig.DeviceName && controllerConfig.DeviceName == device->Name();

            if (controllerConfig.DeviceId >= 0 && controllerConfig.DeviceId < 16)
            {
                matched |= devicesByIndex_[controllerConfig.DeviceId] == device;
            }

            if (!matched) continue;

            if (controllerConfig.Mode.value() == XINPUT_NATIVE)
            {
                continue;
            }

            if (config->EmulatedXInputEnabled == false && controllerConfig.Mode.value() == XINPUT)
            {
                continue;
            }

            if (KeyBinds* keybinds = std::get_if<KeyBinds>(&controllerConfig.Bindings))
            {
                InputState newState = device->Parse(keybinds, rawinput);
                auto inputState = std::make_shared<InputState>(newState);
                std::atomic_store(&controllerConfig.LastState, inputState);
            }
            else
            {
                auto* xinputKeybinds = std::get_if<XInputKeyBinds>(&controllerConfig.Bindings);
                if (!xinputKeybinds) fatal("Config::Bindings neither KeyBinds nor XInputKeyBinds?");
                XINPUT_GAMEPAD inputs = device->ParseXInput(xinputKeybinds, rawinput);
                DWORD id = 0;

                auto oldState = std::atomic_load(&controllerConfig.LastXInputState);
                if (oldState)
                    id = oldState->dwPacketNumber + 1;

                auto newState = std::make_shared<XINPUT_STATE>();
                newState->dwPacketNumber = id;
                newState->Gamepad = inputs;

                std::atomic_store(&controllerConfig.LastXInputState, newState);
            }
        }
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
                std::shared_ptr<InputConfig> config = inputConfig_.load();
                for (auto& controllerIt : config->Controllers)
                {
                    ControllerConfig& controllerConfig = controllerIt.second;
                    if (!controllerConfig.Enabled) continue;

                    bool matched = false;
                    matched |= controllerConfig.DevicePath && controllerConfig.DevicePath == device->Path();
                    matched |= controllerConfig.DeviceName && controllerConfig.DeviceName == device->Name();

                    if (controllerConfig.DeviceId >= 0 && controllerConfig.DeviceId < 16)
                    {
                        matched |= devicesByIndex_[controllerConfig.DeviceId] == device;
                    }

                    if (!matched)
                    {
                        continue;
                    }

                    controllerConfig.LastState.reset();
                    controllerConfig.LastXInputState.reset();

                }
                
                device = nullptr;
            }
        }

        devices_.erase(it);
    }

    void UpdateConfig(std::shared_ptr<InputConfig> newConfig)
    {
        inputConfig_.store(newConfig);
    }

    void DumpDevicesLocked() _Requires_lock_held_(mutex_)
    {
        std::shared_ptr<InputConfig> config = inputConfig_.load();
        for (const auto it : devices_)
        {
            auto device = it.second;
            std::optional<std::string> name = device->Name();
            std::string path = device->Path();

            info("  %s (%s)", name ? name->c_str() : "<unknown>", path.c_str());

            std::vector<std::string> nameMatches;
            std::vector<std::string> pathMatches;

            for (const auto& configIt : config->Controllers)
            {
                const auto& configName = configIt.first;
                const auto& config = configIt.second;

                if (config.DeviceName && config.DeviceName == name)
                {
                    nameMatches.push_back(configName);
                }

                if (config.DevicePath && config.DevicePath == path)
                {
                    pathMatches.push_back(configName);
                }
            }

            if (nameMatches.empty() && pathMatches.empty())
            {
                info("    UNSELECTED");
            }
            else
            {
                info("    name: [%s], path: [%s]", Join(", ", nameMatches).c_str(), Join(", ", pathMatches).c_str());
            }
        }
    }

    void DumpDevices()
    {
        std::lock_guard<std::mutex> lock(mutex_);
        DumpDevicesLocked();
    }

    void DumpConfig()
    {
        info("Config:");
        std::shared_ptr<InputConfig> config = inputConfig_.load();
        info("%s", config->Dump("  ").c_str());
        info("");
    }

  private:
    std::atomic<std::shared_ptr<InputConfig>> inputConfig_ = nullptr;
    std::atomic<std::shared_ptr<InputState>> lastControllerState_ = nullptr;

    std::mutex mutex_;
    _Guarded_by_(mutex_) std::unordered_map<HANDLE, std::shared_ptr<InputDevice>> devices_;
    _Guarded_by_(mutex_) std::array<std::shared_ptr<InputDevice>, 16> devicesByIndex_;
};

static RawInputManager g_inputManager;

void UpdateInputConfig(InputConfig&& newConfig)
{
    auto config = std::make_shared<InputConfig>(std::move(newConfig));
    debug("Updating input config:\n%s", config->Dump("  ").c_str());
    g_inputManager.UpdateConfig(std::move(config));
    g_inputManager.DumpConfig();
    g_inputManager.DumpDevices();
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

DWORD (*XInputGetState_Orig)(DWORD dwUserIndex, XINPUT_STATE* pState);
DWORD (*XInputSetState_Orig)(DWORD dwUserIndex, XINPUT_VIBRATION* pVibration);
DWORD (*XInputGetCapabilities_Orig)(DWORD dwUserIndex, DWORD dwFlags, XINPUT_CAPABILITIES* pCapabilities);


static DWORD XInputGetState_Hook(DWORD dwUserIndex, XINPUT_STATE* pState)
{
    if (globalConfig.Mode == 2 || globalConfig.Mode == 4)
    {
        return ERROR_DEVICE_NOT_CONNECTED;
    }

    if (g_inputManager.HasNativeXInputController())
    {
        return XInputGetState_Orig(dwUserIndex, pState);
    }
    
    if (dwUserIndex != 0)
    {
        return ERROR_DEVICE_NOT_CONNECTED;
    }

    auto result = g_inputManager.GetXInputState();
    if (!result)
    {
        if (g_inputManager.HasXInputController())
        {
            memset(pState, 0, sizeof(*pState));
            return ERROR_SUCCESS;
        }

        return ERROR_DEVICE_NOT_CONNECTED;
    }

    *pState = *result;
    return ERROR_SUCCESS;
}

static DWORD XInputSetState_Hook(DWORD dwUserIndex, XINPUT_VIBRATION* pVibration)
{
    if (globalConfig.Mode == 2 || globalConfig.Mode == 4)
    {
        return ERROR_DEVICE_NOT_CONNECTED;
    }

    if (g_inputManager.HasNativeXInputController())
    {
        return XInputSetState_Orig(dwUserIndex, pVibration);
    }
    
    if (dwUserIndex != 0)
        return ERROR_DEVICE_NOT_CONNECTED;
    if (!g_inputManager.HasXInputController())
        return ERROR_DEVICE_NOT_CONNECTED;
    return ERROR_SUCCESS;
}

static DWORD XInputGetCapabilities_Hook(DWORD dwUserIndex, DWORD dwFlags, XINPUT_CAPABILITIES* pCapabilities)
{
    if (globalConfig.Mode == 2 || globalConfig.Mode == 4)
    {
        return ERROR_DEVICE_NOT_CONNECTED;
    }

    if (g_inputManager.HasNativeXInputController())
    {
        return XInputGetCapabilities_Orig(dwUserIndex, dwFlags, pCapabilities);
    }
    
    if (dwUserIndex != 0)
        return ERROR_DEVICE_NOT_CONNECTED;
    if (!g_inputManager.HasXInputController())
        return ERROR_DEVICE_NOT_CONNECTED;
    pCapabilities->Type = XINPUT_DEVTYPE_GAMEPAD;
    pCapabilities->SubType = XINPUT_DEVSUBTYPE_GAMEPAD;
    pCapabilities->Flags = 0;
    pCapabilities->Gamepad = {}; // ???
    pCapabilities->Vibration.wLeftMotorSpeed = 0;
    pCapabilities->Vibration.wRightMotorSpeed = 0;
    return ERROR_SUCCESS;
}

void InitializeInputHooks()
{
    info("Hooking XInput functions");
    MH_CreateHookApi(L"XINPUT9_1_0.dll", "XInputGetState", XInputGetState_Hook, reinterpret_cast<void**>(&XInputGetState_Orig));
    MH_CreateHookApi(L"XINPUT9_1_0.dll", "XInputSetState", XInputSetState_Hook, reinterpret_cast<void**>(&XInputSetState_Orig));
    MH_CreateHookApi(L"XINPUT9_1_0.dll", "XInputGetCapabilities", XInputGetCapabilities_Hook, reinterpret_cast<void**>(&XInputGetCapabilities_Orig));
}
