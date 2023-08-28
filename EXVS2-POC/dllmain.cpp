#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <string>
#include <thread>

#include "AmAuthEmu.h"
#include "GameHooks.h"
#include "JvsEmu.h"
#include "WindowedDxgi.h"
#include "INIReader.h"
#include "VirtualKeyMapping.h"
#include "configs.h"

config_struct ReadConfigs(INIReader reader) {
    config_struct config {};

    // config reading
    config.Windowed = reader.GetBoolean("config", "windowed", false);
    config.InputMode = reader.Get("config", "InputMode", "Keyboard");
    config.PcbId = reader.Get("config", "PcbId", "ABLN1110001");
    config.Serial = reader.Get("config", "serial", "284311110001");
    config.Mode = static_cast<uint8_t>(reader.GetInteger("config", "mode", 2));
    config.IpAddress = reader.Get("config", "IpAddress", "192.168.50.239");
    config.Gateway = reader.Get("config", "Gateway", "192.168.50.1");
    config.SubnetMask = reader.Get("config", "SubnetMask", "255.255.255.0");
    config.PrimaryDNS = reader.Get("config", "DNS", "8.8.8.8");
    config.TenpoRouter = reader.Get("config", "TenpoRouter", "192.168.50.1");
    config.AuthServerIp = reader.Get("config", "AuthIP", "127.0.0.1");
    config.ServerAddress = reader.Get("config", "Server", "127.0.0.1");
    config.RegionCode = reader.Get("config", "Region", "1");
    
    // key bind config reading
    jvs_key_bind key_bind;
    std::string keyMapPlaceholder;

    keyMapPlaceholder = reader.Get("keybind", "KillProcess", "Esc");
    key_bind.KillProcess = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Test", "T");
    key_bind.Test = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Start", "O");
    key_bind.Start = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Service", "S");
    key_bind.Service = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Coin", "M");
    key_bind.Coin = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Up", "UpArr");
    key_bind.Up = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Left", "LeftArr");
    key_bind.Left = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Down", "DownArr");
    key_bind.Down = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Right", "RightArr");
    key_bind.Right = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Button1", "Z");
    key_bind.Button1 = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Button2", "X");
    key_bind.Button2 = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Button3", "C");
    key_bind.Button3 = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Button4", "V");
    key_bind.Button4 = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Card", "P");
    key_bind.Card = findKeyByValue(keyMapPlaceholder);

    key_bind.DirectInputDeviceId = reader.GetInteger("keybind", "DirectInputDeviceId", 16);

    key_bind.ArcadeButton1 = reader.GetInteger("keybind", "ArcadeButton1", 1);
    
    key_bind.ArcadeButton2 = reader.GetInteger("keybind", "ArcadeButton2", 2);
    
    key_bind.ArcadeButton3 = reader.GetInteger("keybind", "ArcadeButton3", 3);

    key_bind.ArcadeButton4 = reader.GetInteger("keybind", "ArcadeButton4", 4);

    key_bind.ArcadeStartButton = reader.GetInteger("keybind", "ArcadeStartButton", 5);
    
    key_bind.ArcadeCoin = reader.GetInteger("keybind", "ArcadeCoin", 6);

    key_bind.ArcadeTest = reader.GetInteger("keybind", "ArcadeTest", 7);

    key_bind.ArcadeCard = reader.GetInteger("keybind", "ArcadeCard", 8);

    key_bind.UseKeyboardSupportKeyInDirectInput = reader.GetBoolean("keybind", "UseKeyboardSupportKeyInDirectInput", true);

    config.KeyBind = key_bind;
    return config;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    // Read config
    INIReader reader("config.ini");

    if (reader.ParseError() == 0)
    {
        globalConfig = ReadConfigs(reader);
    }

    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        {
            InitAmAuthEmu();
            InitializeHooks();
            InitializeJvs();
            InitDXGIWindowHook();
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
