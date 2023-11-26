#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include <algorithm>
#include <string>
#include <string_view>
#include <thread>
#include <vector>

#include "Configs.h"
#include "INIReader.h"
#include "log.h"

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

    config->Windowed = reader.GetBoolean("config", "windowed", false);

    std::string modeString = reader.Get("config", "mode", "LM");
    if (modeString == "1" || _stricmp("client", modeString.c_str()) == 0)
    {
        config->Mode = 1;
    }
    else if (modeString == "2" || _stricmp("lm", modeString.c_str()) == 0)
    {
        config->Mode = 2;
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
    if (config->Serial.size() == 4)
    {
        config->Serial = (config->Mode == 1 ? "28431411" : "28431111") + config->Serial;
    }

    bool validLength = config->Serial.size() == 12;
    bool validClientPrefix = config->Serial.starts_with("28431411") || config->Serial.starts_with("28431311");
    bool validLMPrefix = config->Serial.starts_with("28431111");
    if (config->Mode == 1 && (!validLength || !validClientPrefix))
    {
        fatal("invalid serial: expected serial of format 28431411XXXX/28431311XXXX for client");
    }
    else if (config->Mode == 2 && (!validLength || !validLMPrefix))
    {
        fatal("invalid serial: expected serial of format 28431111XXXX for LM");
    }

    config->PcbId = reader.GetOptional("config", "PcbId").value_or("ABLN1" + config->Serial.substr(5));

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

    config->KeyboardEnabled = reader.GetBoolean("keyboard", "Enabled", true);
    config->KeyboardBindings = keyboard;

    config->ControllerEnabled = reader.GetBoolean("controller", "Enabled", true);
    // TODO: This should take a GUID instead of an index.
    config->ControllerDeviceId = reader.GetInteger("controller", "DeviceId", 16);
    config->ControllerBindings = dinput;
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
}
