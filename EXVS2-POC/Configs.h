#pragma once

#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <xinput.h>

#include <filesystem>
#include <memory>
#include <optional>
#include <string>
#include <unordered_map>
#include <variant>
#include <vector>

#include "VirtualKeyMapping.h"

struct AudioConfig
{
    bool DisableHook = false;
    std::optional<std::string> DeviceId;
    std::optional<std::string> DeviceName;
};

struct DisplayConfig
{
    std::string Resolution;
    bool FramerateLimit = false;
    bool HideFreeplay = false;
    bool HideCreditCount = false;
};

struct TimeBoosterConfig
{
    bool FreeRentalMode = false;
    bool FreeCost = false;
    bool ExtendRentalMode = false;
    bool ExtendGameModeSelection = false;
    bool ExtendMsSelection = false;
    bool ExtendTriadCourseSelection = false;
    bool ExtendTrainingModeSelection = false;
    bool ExtendModeUnselect = false;
    bool EnableStaffRollInRental = false;
};

struct StartupConfig
{
    bool UseIoBoard = false;
    bool Windowed = false;
    bool BorderlessWindow = false;
    bool EnableDebugInPcb = false;
    bool EnableInGamePerformanceMeter = false;
    bool EnableQuickSkinChange = false;
    bool DisableSocketHook = false;
    uint8_t Mode = 0;

    std::string BootToken;
    std::string SerialPrefix;
    std::string Serial;
    std::string PcbId;
    std::string AuthServerIp;
    std::optional<std::string> InterfaceName;
    std::optional<std::string> IpAddress;
    std::optional<std::string> SubnetMask;
    std::optional<std::string> Gateway;
    std::optional<std::string> TenpoRouter;
    std::optional<std::string> PrimaryDNS;
    std::string ServerAddress;
    std::string RegionCode;
    std::string ShopName;
    std::string ShopNickname;
    std::string OpeningScreenSkip;
    
    bool UseRealCardReader = false;
    bool UsePyBanapassReader = false;
    bool AutomaticCardButton = false;
    bool SkipAutoCreateForPyBanapassReader = true;
    std::string CardReaderComPort;
    std::string CardFileBasePath;

    DisplayConfig Display;
    AudioConfig Audio;
    TimeBoosterConfig TimeBooster;
};

extern StartupConfig globalConfig;

// X macro that enumerates all of the keybinds, along with default keyboard and directinput values
#define KEYBINDS()                                                                                                     \
    KEYBIND(Up, "UpArr", "")                                                                                           \
    KEYBIND(Down, "DownArr", "")                                                                                       \
    KEYBIND(Left, "LeftArr", "")                                                                                       \
    KEYBIND(Right, "RightArr", "")                                                                                     \
    KEYBIND(A, "Z", "1")                                                                                               \
    KEYBIND(B, "X", "4")                                                                                               \
    KEYBIND(C, "C", "6")                                                                                               \
    KEYBIND(D, "V", "2")                                                                                               \
    KEYBIND(Start, "1", "")                                                                                            \
    KEYBIND(Coin, "M", "")                                                                                             \
    KEYBIND(Card, "P", "")                                                                                             \
    KEYBIND(Test, "T", "")                                                                                             \
    KEYBIND(Service, "S", "")                                                                                          \
    KEYBIND(Kill, "Esc", "")

struct KeyBinds
{
#define KEYBIND(name, default_keyboard, default_dinput) std::vector<int> name;
    KEYBINDS()
#undef KEYBIND

    std::string Dump(const std::string& prefix = "") const;
};

struct InputState;

#define XINPUT_KEYBINDS()                                                                                              \
    XINPUT_KEYBIND(A, XINPUT_GAMEPAD_A, 0, "2")                                                                        \
    XINPUT_KEYBIND(B, XINPUT_GAMEPAD_B, 0, "3")                                                                        \
    XINPUT_KEYBIND(X, XINPUT_GAMEPAD_X, 0, "1")                                                                        \
    XINPUT_KEYBIND(Y, XINPUT_GAMEPAD_Y, 0, "4")                                                                        \
    XINPUT_KEYBIND(L1, XINPUT_GAMEPAD_LEFT_SHOULDER, 0, "5")                                                           \
    XINPUT_KEYBIND(L2, 0, -1, "7")                                                                                     \
    XINPUT_KEYBIND(L3, XINPUT_GAMEPAD_LEFT_THUMB, 0, "11")                                                             \
    XINPUT_KEYBIND(R1, XINPUT_GAMEPAD_RIGHT_SHOULDER, 0, "6")                                                          \
    XINPUT_KEYBIND(R2, 0, 1, "8")                                                                                      \
    XINPUT_KEYBIND(R3, XINPUT_GAMEPAD_RIGHT_THUMB, 0, "12")                                                            \
    XINPUT_KEYBIND(Start, XINPUT_GAMEPAD_START, 0, "10")                                                               \
    XINPUT_KEYBIND(Card, XINPUT_GAMEPAD_BACK, 0, "9")

struct XInputKeyBinds
{
#define XINPUT_KEYBIND(name, xinput_button, xinput_trigger, default_bind) std::vector<int> name;
    XINPUT_KEYBINDS()
#undef XINPUT_KEYBIND

    std::string Dump(const std::string& prefix = "") const;
};

struct ControllerConfig
{
    // Config options
    bool Enabled;
    int DeviceId;
    std::optional<std::string> DevicePath;
    std::optional<std::string> DeviceName;
    std::optional<std::string> Mode;

    std::variant<KeyBinds, XInputKeyBinds> Bindings;

    // Scratch data
    std::shared_ptr<InputState> LastState;
    std::shared_ptr<XINPUT_STATE> LastXInputState;

    std::string Dump(const std::string& name, const std::string& prefix = "") const;
};


struct InputConfig
{
    bool KeyboardEnabled;
    bool EmulatedXInputEnabled;
    KeyBinds KeyboardBindings;

    std::unordered_map<std::string, ControllerConfig> Controllers;

    std::string Dump(const std::string& prefix = "") const;
};

std::filesystem::path GetBasePath();
void InitializeConfig();
void UpdateInputConfig(InputConfig&& newConfig);
