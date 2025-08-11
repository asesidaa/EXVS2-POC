#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include <algorithm>
#include <chrono>
#include <string>
#include <string_view>
#include <thread>
#include <vector>

#include "Configs.h"
#include "INIReader.h"
#include "log.h"
#include "Version.h"
#include "util.h"

using namespace std::chrono_literals;

StartupConfig globalConfig;

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

static void ReadStartupConfig(StartupConfig* config, INIReader& reader)
{
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

    config->BootToken = reader.Get("config", "BootToken", "");
    config->UseIoBoard = reader.GetBoolean("config", "useioboard", false);
    config->Windowed = reader.GetBoolean("config", "windowed", false);
    config->BorderlessWindow = reader.GetBoolean("config", "borderlesswindow", false);
    config->EnableDebugInPcb = reader.GetBoolean("config", "enabledebuginpcb", false);
    config->EnableInGamePerformanceMeter = reader.GetBoolean("config", "enableingameperformancemeter", false);
    config->EnableQuickSkinChange = reader.GetBoolean("config", "enablequickskinChange", false);
    config->DisableSocketHook = reader.GetBoolean("config", "disablesockethook", false);

    std::string modeString = reader.Get("config", "mode", "LM");
    
    uint8_t pcbMode = 1;
    uint8_t lmMode = 2;
    
    if (modeString == "1" || _stricmp("client", modeString.c_str()) == 0)
    {
        config->Mode = pcbMode;
    }
    else if (modeString == "2" || _stricmp("lm", modeString.c_str()) == 0)
    {
        config->Mode = lmMode;
    }
    else
    {
        fatal("invalid mode: %s", modeString.c_str());
    }
    
    config->Serial = reader.Get("config", "serial", "0001");
    if (config->Serial.size() != 4 && config->Serial.size() != 12)
    {
        fatal("invalid serial: expected 4 or 12 digit serial number");
    }

    const char* serialPrefixLM = nullptr;
    const char* serialPrefixClient = nullptr;
    const char* serialPrefixClientAlter = nullptr;
    const char* pcbIdPrefix = nullptr;

    switch (GetGameVersion()) {
        case VS2_400:
            serialPrefixLM = "28111101";
            serialPrefixClient = "28111401";
            serialPrefixClientAlter = "28111301";
            pcbIdPrefix = "ABLN";
            break;
        case XBoost_450:
            serialPrefixLM = "28431111";
            serialPrefixClient = "28431411";
            serialPrefixClientAlter = "28431311";
            pcbIdPrefix = "ABLN";
            break;
        case Overboost_480:
            serialPrefixLM = "28681123";
            serialPrefixClient = "28681423";
            serialPrefixClientAlter = "28681323";
            pcbIdPrefix = "ABLN";
            break;
        default:
            fatal("Unknown game version: %d", GetGameVersion());
    }

    bool isPcb = config->Mode == pcbMode;
    bool isLM = config->Mode == lmMode;

    if (config->Serial.size() == 4)
    {
        if(isPcb)
        {
            config->Serial = serialPrefixClient + config->Serial;
        }
        else
        {
            config->Serial = serialPrefixLM + config->Serial;
        }
    }

    bool validLength = config->Serial.size() == 12;
    bool validClientPrefix = config->Serial.starts_with(serialPrefixClient) || config->Serial.starts_with(serialPrefixClientAlter);
    bool validLMPrefix = config->Serial.starts_with(serialPrefixLM);
    
    if (isPcb && (!validLength || !validClientPrefix))
    {
        fatal("invalid serial: expected serial of format %sXXXX/%sXXXX for client", serialPrefixClient,
              serialPrefixClientAlter);
    }
    else if (isLM && (!validLength || !validLMPrefix))
    {
        fatal("invalid serial: expected serial of format %sXXXX for LM", serialPrefixLM);
    }

    config->PcbId = reader.GetOptional("config", "PcbId").value_or(pcbIdPrefix + config->Serial.substr(5));

    // These will get filled in by InitializeSocketHooks.
    config->InterfaceName = reader.GetOptional("config", "InterfaceName");
    config->IpAddress = reader.GetOptional("config", "IpAddress");
    config->Gateway = reader.GetOptional("config", "Gateway");
    config->TenpoRouter = reader.GetOptional("config", "TenpoRouter");
    config->SubnetMask = reader.GetOptional("config", "SubnetMask");
    config->PrimaryDNS = reader.GetOptional("config", "DNS");

    config->AuthServerIp = reader.Get("config", "AuthIP", "127.0.0.1");
    config->ServerAddress = reader.Get("config", "Server", "127.0.0.1");
    config->RegionCode = reader.Get("config", "Region", "1");
    config->ShopName = reader.Get("config", "ShopName", "NEXTREME");
    config->ShopNickname = reader.Get("config", "ShopNickname", "NEXTREME");

    config->UseRealCardReader = reader.GetBoolean("config", "userealcardreader", false);
    config->UsePyBanapassReader = reader.GetBoolean("config", "UsePyBanapassReader", false);
    config->SkipAutoCreateForPyBanapassReader = reader.GetBoolean("config", "SkipAutoCreateForPyBanapassReader", true);
    config->AutomaticCardButton = reader.GetBoolean("config", "AutomaticCardButton", false);
    
    config->CardReaderComPort = reader.Get("config", "cardreadercomport", "COM4");
    config->CardFileBasePath = reader.Get("config", "CardFileBasePath", "");

    if(config->UseRealCardReader == true && config->CardReaderComPort == "COM3")
    {
        fatal("COM3 is reserved for Controller and cannot be used as Card Reader COM Port");
    }

    config->Audio.DisableHook = reader.GetBoolean("audio", "DisableHook", false);

    config->Audio.DeviceId = reader.GetOptional("audio", "DeviceId");
    if (!config->Audio.DeviceId)
    {
        config->Audio.DeviceId = reader.GetOptional("audio", "Device");
    }

    config->Audio.DeviceName = reader.GetOptional("audio", "DeviceName");

    config->Display.Resolution = reader.Get("display", "resolution", "1080p");

    if(config->Display.Resolution != "144p" && config->Display.Resolution != "240p"
        && config->Display.Resolution != "480p" && config->Display.Resolution != "720p"
        && config->Display.Resolution != "900p"
        && config->Display.Resolution != "1080p" && config->Display.Resolution != "2k"
        && config->Display.Resolution != "4k" && config->Display.Resolution != "8k")
    {
        fatal("Unsupported Resolution Setting %s", config->Display.Resolution.c_str());
    }
    
    if (!config->Windowed)
    {
        config->Display.FramerateLimit = false;
    }
    else
    {
        config->Display.FramerateLimit = reader.GetBoolean("display", "FramerateLimit", false);
    }

    auto isBorderless = config->Windowed == true && config->BorderlessWindow == true;

    if(config->Display.Resolution == "8k" && isBorderless == false)
    {
        fatal("%s is supported in Borderless Window mode only!", config->Display.Resolution.c_str());
    }

    config->OpeningScreenSkip = reader.Get("config", "OpeningScreenSkip", "SkipReminder");

    if(config->OpeningScreenSkip != "None" && config->OpeningScreenSkip != "SkipReminder" &&
        config->OpeningScreenSkip != "SkipBrand" && config->OpeningScreenSkip != "SkipAll")
    {
        fatal("%s is an unsupported Opening Screen Skip Mode", config->OpeningScreenSkip.c_str());
    }
}

static void ReadInputConfig(InputConfig* config, INIReader& reader)
{
    // key bind config reading
    KeyBinds keyboard;
#define KEYBIND(name, kb_default, dinput_default)                                                                      \
    {                                                                                                                  \
        std::vector<std::string> keys = Split(reader.Get("keyboard", #name, kb_default), ',');                         \
        for (const auto& key : keys)                                                                                   \
        {                                                                                                              \
            int val = findKeyByValue(key);                                                                             \
            if (val != -1)                                                                                             \
                keyboard.name.push_back(val);                                                                          \
            else                                                                                                       \
                fatal("failed to interpret key '%s'", key.c_str());                                                    \
        }                                                                                                              \
    }
    KEYBINDS()
#undef KEYBIND

    config->KeyboardEnabled = reader.GetBoolean("keyboard", "Enabled", true);
    config->KeyboardBindings = keyboard;

    config->EmulatedXInputEnabled = reader.GetBoolean("xinput", "EmulatedXInputEnabled", false);

    for (const std::string& section : reader.Sections())
    {
        if (section != "controller" && !section.starts_with("controller-"))
        {
            continue;
        }

        ControllerConfig controllerConfig;
        controllerConfig.Enabled = reader.GetBoolean(section, "Enabled", true);
        controllerConfig.DeviceId = reader.GetInteger(section, "DeviceId", 16);
        controllerConfig.DevicePath = reader.GetOptional(section, "Path");
        controllerConfig.DeviceName = reader.GetOptional(section, "Name");

        if (reader.Get(section, "Mode", "directinput") == "xinput-native")
        {
            controllerConfig.Mode = "xinput-native";
            config->Controllers.emplace(std::move(section), controllerConfig);
            continue;
        }
        
        if (reader.Get(section, "Mode", "directinput") == "xinput" && config->EmulatedXInputEnabled == false)
        {
            controllerConfig.Mode = "xinput-native";
            config->Controllers.emplace(std::move(section), controllerConfig);
            continue;
        }

        if (reader.Get(section, "Mode", "directinput") == "xinput")
        {
            controllerConfig.Mode = "xinput";
            XInputKeyBinds xinput;
#define XINPUT_KEYBIND(name, xinput_button, xinput_trigger, default_bind)          \
            {                                                                      \
                std::vector<std::string> keys =                                    \
                    Split(reader.Get(section, #name, default_bind), ',');          \
                for (const auto &key : keys)                                       \
                {                                                                  \
                  xinput.name.push_back(atoi(key.c_str()));                        \
                }                                                                  \
            }
            XINPUT_KEYBINDS()
#undef XINPUT_KEYBIND
            controllerConfig.Bindings = xinput;
        }
        else
        {
            controllerConfig.Mode = "directinput";
            KeyBinds dinput;
#define KEYBIND(name, kb_default, dinput_default)                               \
            {                                                                      \
                std::vector<std::string> keys =                                    \
                    Split(reader.Get(section, #name, dinput_default), ',');        \
                for (const auto &key : keys)                                       \
                {                                                                  \
                  dinput.name.push_back(atoi(key.c_str()));                        \
                }                                                                  \
            }
            KEYBINDS()
#undef KEYBIND
            controllerConfig.Bindings = dinput;
        }
        config->Controllers.emplace(std::move(section), controllerConfig);
    }
}

std::string KeyBinds::Dump(const std::string& prefix) const
{
    std::string result;
#define KEYBIND(name, keyboard, dinput)                                                                                \
    if (!name.empty())                                                                                                 \
    {                                                                                                                  \
        result += prefix;                                                                                              \
        result += #name;                                                                                               \
        result += " = ";                                                                                               \
        result += Join(", ", name);                                                                                    \
        result += "\n";                                                                                                \
    }
    KEYBINDS();
#undef KEYBIND
    return result;
}

std::string XInputKeyBinds::Dump(const std::string& prefix) const
{
    std::string result;
#define XINPUT_KEYBIND(name, xinput_button, xinput_trigger, default_bind)                                              \
    if (!name.empty())                                                                                                 \
    {                                                                                                                  \
        result += prefix;                                                                                              \
        result += #name;                                                                                               \
        result += " = ";                                                                                               \
        result += Join(", ", name);                                                                                    \
        result += "\n";                                                                                                \
    }
    XINPUT_KEYBINDS();
#undef XINPUT_KEYBIND
    return result;
}

std::string ControllerConfig::Dump(const std::string& name, const std::string& prefix) const
{
    std::string result;
    result += prefix + "[" + name + "]\n";
    result += prefix + "Enabled = ";
    result += Enabled ? "true" : "false";
    result += "\n";

    if (DevicePath)
    {
        result += prefix;
        result += "Path = ";
        result += *DevicePath;
        result += "\n";
    }

    if (DeviceName)
    {
        result += prefix;
        result += "Name = ";
        result += *DeviceName;
        result += "\n";
    }

    result += prefix;
    result += "DeviceId = ";
    result += std::to_string(DeviceId);
    result += "\n";
    
    if (const KeyBinds* binds = std::get_if<KeyBinds>(&Bindings))
    {
        result += prefix;
        result += "Mode = directinput\n";
        result += binds->Dump(prefix);
    }
    else if (const XInputKeyBinds* binds = std::get_if<XInputKeyBinds>(&Bindings))
    {
        result += prefix;
        result += "Mode = xinput\n";
        result += binds->Dump(prefix);
    }
    else
    {
        result += prefix;
        result += "<ERROR: no bindings>";
    }

    return result;
}

std::string InputConfig::Dump(const std::string& prefix) const
{
    std::string result;
    result += prefix + "[keyboard]\n";

    result += prefix + "Enabled = ";
    result += KeyboardEnabled ? "true" : "false";
    result += "\n";
    
    result += KeyboardBindings.Dump(prefix);
    result += "\n";

    result += "\n";

    result += prefix + "[xinput]\n";

    result += prefix + "EmulatedXInputEnabled = ";
    result += EmulatedXInputEnabled ? "true" : "false";
    result += "\n";
    result += "\n";

    for (const auto& it : Controllers) {
        result += it.second.Dump(it.first, prefix);
        result += "\n";
        result += "\n";
    }

    return result;
}

static void ConfigMonitorThread()
{
    std::filesystem::path basePath = GetBasePath();
    std::string configPath = (basePath / "config.ini").string();

    HANDLE dir = CreateFileW(basePath.wstring().c_str(), GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, nullptr,
                             OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL | FILE_FLAG_BACKUP_SEMANTICS, nullptr);
    if (dir == INVALID_HANDLE_VALUE)
        fatal("failed to open handle to %s", basePath.string().c_str());

    while (true)
    {
        // Sleep for a bit, to give the writer a chance to finish.
        std::this_thread::sleep_for(100ms);
        INIReader reader(configPath);

        InputConfig inputConfig;

        ReadInputConfig(&inputConfig, reader);
        UpdateInputConfig(std::move(inputConfig));

        union {
            char bytes[4096];
            DWORD align;
        } buf;
        DWORD bytesReturned = 0;

        bool configChanged = false;
        while (!configChanged)
        {
            if (!ReadDirectoryChangesW(dir, buf.bytes, sizeof(buf), false, FILE_NOTIFY_CHANGE_LAST_WRITE,
                                       &bytesReturned, nullptr, nullptr))
            {
                err("ReadDirectoryChangesW failed, stopping config monitor thread");
                return;
            }

            if (bytesReturned == 0)
                continue;

            char* p = buf.bytes;
            auto* info = reinterpret_cast<FILE_NOTIFY_INFORMATION*>(p);
            while (true)
            {
                if (info->Action == FILE_ACTION_ADDED || info->Action == FILE_ACTION_MODIFIED ||
                    info->Action == FILE_ACTION_RENAMED_NEW_NAME)
                {
                    std::wstring filename(info->FileName, info->FileNameLength / 2);
                    if (_wcsicmp(L"config.ini", filename.c_str()) == 0)
                    {
                        info("Detected change to config.ini");
                        configChanged = true;
                        break;
                    }
                    else
                    {
                        info("Ignoring changed file: '%S'", filename.c_str());
                    }
                }

                if (info->NextEntryOffset == 0)
                    break;
                p += info->NextEntryOffset;
                info = reinterpret_cast<FILE_NOTIFY_INFORMATION*>(p);
            }
        }
    }
}

void InitializeConfig()
{
    // FIXME: INIReader doesn't handle Unicode paths.
    std::filesystem::path basePath = GetBasePath();
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

    ReadStartupConfig(&globalConfig, reader);

    InputConfig inputConfig;
    ReadInputConfig(&inputConfig, reader);
    UpdateInputConfig(std::move(inputConfig));

    std::thread(ConfigMonitorThread).detach();
}
