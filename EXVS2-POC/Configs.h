#pragma once

#include <filesystem>
#include <optional>
#include <string>
#include <vector>

#include "VirtualKeyMapping.h"

struct StartupConfig
{
    bool Windowed = false;
    bool BorderlessWindow = false;
    bool EnableInGamePerformanceMeter = false;
    bool DisableSocketHook = false;
    uint8_t Mode = 0;

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

    bool UseRealCardReader = false;
    std::string CardReaderComPort;
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
#define KEYBIND(name, keyboard, dinput) std::vector<int> name;
    KEYBINDS()
#undef KEYBIND

    std::string Dump(const std::string& prefix = "");
};

struct InputConfig
{
    bool KeyboardEnabled;
    KeyBinds KeyboardBindings;

    bool ControllerEnabled;
    int ControllerDeviceId;
    std::optional<std::string> ControllerPath;
    KeyBinds ControllerBindings;

    std::string Dump(const std::string& prefix = "");
};

std::filesystem::path GetBasePath();
void InitializeConfig();
void UpdateInputConfig(InputConfig&& newConfig);
