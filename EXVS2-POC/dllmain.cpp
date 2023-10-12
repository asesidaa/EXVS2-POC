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
    if (g_logLevel != LogLevel::NONE)
    {
        AllocConsole();

        FILE* dummy;
        freopen_s(&dummy, "CONIN$", "r", stdin);
        freopen_s(&dummy, "CONOUT$", "w", stderr);
        freopen_s(&dummy, "CONOUT$", "w", stdout);
    }

    config.Windowed = reader.GetBoolean("config", "windowed", false);

    std::string modeString = reader.Get("config", "mode", "LM");
    if (modeString == "1" || _stricmp("client", modeString.c_str()) == 0) {
      config.Mode = 1;
    } else if (modeString == "2" || _stricmp("lm", modeString.c_str()) == 0) {
      config.Mode = 2;
    } else {
      fatal("invalid mode: %s", modeString.c_str());
    }

    config.Serial = reader.Get("config", "serial", "0001");
    if (config.Serial.size() != 4 && config.Serial.size() != 12) {
      fatal("invalid serial: expected 4 or 12 digit serial number");
    }
    if (config.Serial.size() == 4) {
      config.Serial = (config.Mode == 1 ? "28431411" : "28431111") + config.Serial;
    }

    bool validLength = config.Serial.size() == 12;
    if (config.Mode == 1 && (!validLength || !config.Serial.starts_with("28431411"))) {
      fatal("invalid serial: expected serial of format 28431411XXXX for client");
    } else if (config.Mode == 2 && (!validLength || !config.Serial.starts_with("28431111"))) {
      fatal("invalid serial: expected serial of format 28431111XXXX for LM");
    }

    config.PcbId = reader.GetOptional("config", "PcbId").value_or("ABLN1" + config.Serial.substr(5));

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
#define KEYBIND(name, kb_default, dinput_default)                                                                      \
    {                                                                                                                  \
        std::vector<std::string> keys = Split(reader.Get("controller", #name, dinput_default), ',');                   \
        for (const auto& key : keys)                                                                                   \
        {                                                                                                              \
            dinput.name.push_back(atoi(key.c_str()));                                                                  \
        }                                                                                                              \
    }
    KEYBINDS()
#undef KEYBIND

    int keyboardEnabled = reader.GetBoolean("keyboard", "Enabled", true);
    int controllerEnabled = reader.GetBoolean("controller", "Enabled", true);
    config.InputMode = static_cast<InputMode>(controllerEnabled << 1 | keyboardEnabled);

    // TODO: This should take a GUID instead of an index.
    config.DirectInputDeviceId = reader.GetInteger("controller", "DeviceId", 16);
    config.DirectInputBindings = dinput;
    config.KeyboardBindings = keyboard;
    return config;
}

std::filesystem::path GetBasePath()
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
