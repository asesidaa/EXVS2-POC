#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <shellapi.h>

#include <algorithm>
#include <chrono>
#include <filesystem>
#include <string>
#include <string_view>
#include <thread>

#include <determinize/determinize.h>
#include <MinHook.h>

#include "AmAuthEmu.h"
#include "Configs.h"
#include "GameHooks.h"
#include "INIReader.h"
#include "JvsEmu.h"
#include "log.h"
#include "PatchTargets.h"
#include "SocketHooks.h"
#include "VirtualKeyMapping.h"
#include "WindowedDxgi.h"

using namespace std::chrono_literals;

static std::vector<std::string> Split(const std::string& string, char delimiter)
{
    std::string_view str = string;
    std::vector<std::string> result;

    while (true)
    {
        auto it = std::find(str.begin(), str.end(), delimiter);
        result.emplace_back(str.begin(), it);

        if (it == str.end())
            break;

        str = str.substr(result.back().size() + 1);
    }

    return result;
}

static config_struct ReadConfigs(INIReader reader) {
    config_struct config{};

    // config reading
    std::string logLevel = reader.Get("config", "log", "none");
    if (_stricmp("trace", logLevel.c_str()) == 0)
    {
        g_logLevel = LogLevel::TRACE;
    }
    else if (_stricmp("debug", logLevel.c_str()) == 0)
    {
        g_logLevel = LogLevel::DEBUG;
    }
    else if (_stricmp("info", logLevel.c_str()) == 0)
    {
        g_logLevel = LogLevel::INFO;
    }
    else if (_stricmp("warn", logLevel.c_str()) == 0)
    {
        g_logLevel = LogLevel::WARN;
    }
    else if (_stricmp("error", logLevel.c_str()) == 0)
    {
        g_logLevel = LogLevel::ERR;
    }
    else if (_stricmp("none", logLevel.c_str()) == 0)
    {
        g_logLevel = LogLevel::NONE;
    }
    else
    {
        fatal("Unknown log level '%s'", logLevel.c_str());
    }

    config.Windowed = reader.GetBoolean("config", "windowed", false);
    config.PcbId = reader.Get("config", "PcbId", "ABLN1110001");
    config.Serial = reader.Get("config", "serial", "284311110001");
    config.Mode = static_cast<uint8_t>(reader.GetInteger("config", "mode", 2));

    // These will get filled in by InitializeSocketHooks.
    config.InterfaceName = reader.GetOptional("config", "InterfaceName");
    config.IpAddress = reader.GetOptional("config", "IpAddress");
    config.Gateway = reader.GetOptional("config", "Gateway");
    config.TenpoRouter = reader.GetOptional("config", "TenpoRouter");
    config.SubnetMask = reader.GetOptional("config", "SubnetMask");
    config.PrimaryDNS = reader.GetOptional("config", "DNS");

    config.AuthServerIp = reader.Get("config", "AuthIP", "127.0.0.1");
    config.ServerAddress = reader.Get("config", "Server", "127.0.0.1");
    config.RegionCode = reader.Get("config", "Region", "1");

    // key bind config reading
    KeyBinds keyboard;
#define KEYBIND(name, kb_default, dinput_default)                                               \
    {                                                                                           \
        std::vector<std::string> keys = Split(reader.Get("keyboard", #name, kb_default), ',');  \
        for (const auto& key : keys) {                                                          \
            int val = findKeyByValue(key);                                                      \
            if (val != -1) keyboard.name.push_back(val);                                        \
            else fatal("failed to interpret key '%s'", key.c_str());                            \
        }                                                                                       \
    }
    KEYBINDS()
#undef KEYBIND

    KeyBinds dinput;
#define KEYBIND(name, kb_default, dinput_default)                                                   \
    {                                                                                               \
        std::vector<std::string> keys = Split(reader.Get("dinput", #name, dinput_default), ',');    \
        for (const auto& key : keys) {                                                              \
            dinput.name.push_back(atoi(key.c_str()));                                               \
        }                                                                                           \
    }
    KEYBINDS()
#undef KEYBIND

    std::string inputMode = reader.Get("config", "InputMode", "Keyboard");
    if (inputMode == "None")
    {
        config.InputMode = InputModeNone;
    }
    else if (inputMode == "Keyboard")
    {
        config.InputMode = InputModeKeyboard;
    }
    else if (inputMode == "DirectInputOnly")
    {
        config.InputMode = InputModeDirectInput;
    }
    else if (inputMode == "DirectInput")
    {
        config.InputMode = InputModeBoth;
    }
    else
    {
        fatal("unknown InputMode: %s (supported values: None, Keyboard, DirectInputOnly, DirectInput)", inputMode.c_str());
    }

    // TODO: This should take a GUID instead of an index.
    config.DirectInputDeviceId = reader.GetInteger("dinput", "DeviceId", 16);
    config.DirectInputBindings = dinput;
    config.KeyboardBindings = keyboard;
    return config;
}

static std::filesystem::path GetBasePath()
{
    int argc;
    LPWSTR* argv = CommandLineToArgvW(GetCommandLineW(), &argc);
    switch (argc)
    {
    case 0:
        fatal("Empty commandline");
    case 1:
        return std::filesystem::current_path();
    case 2:
        return std::filesystem::path(argv[1]);
    default:
        fatal("Invalid commandline (expected only a path, argc = %d)", argc);
    }

}
BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        {
            std::filesystem::path basePath = GetBasePath();

            // FIXME: INIReader doesn't handle Unicode paths.
            INIReader reader((basePath / "config.ini").string());
            int rc = reader.ParseError();
            if (rc == -1)
            {
                fatal("Failed to open config.ini");
            }
            else if (rc > 0)
            {
                fatal("Failed to parse config.ini: error on line %d", reader.ParseError());
            }

            globalConfig = ReadConfigs(reader);

            if (g_logLevel != LogLevel::NONE)
            {
                AllocConsole();

                FILE* dummy;
                freopen_s(&dummy, "CONIN$", "r", stdin);
                freopen_s(&dummy, "CONOUT$", "w", stderr);
                freopen_s(&dummy, "CONOUT$", "w", stdout);
            }

            MH_Initialize();
            InitializeSocketHooks();
            InitAmAuthEmu();
            InitializeHooks(std::move(basePath));
            InitializeJvs();
            InitDXGIWindowHook();
            MH_EnableHook(MH_ALL_HOOKS);

            auto base = reinterpret_cast<char*>(GetModuleHandle(nullptr));
            std::vector<void*> patch_targets;
            for (auto offset : PATCH_TARGETS) {
              patch_targets.push_back(base + offset);
            }

            auto before = std::chrono::steady_clock::now();
            determinize::Determinize(std::move(patch_targets), base);
            auto after = std::chrono::steady_clock::now();

            printf(
                "Patched %zu floating point approximations with deterministic "
                "implementations in %dms\n",
                PATCH_TARGETS.size(),
                static_cast<int>((after - before) / 1.0ms));
        }
        break;
    case DLL_THREAD_ATTACH:  // NOLINT(bugprone-branch-clone)
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    default: break;
    }

    return TRUE;
}
