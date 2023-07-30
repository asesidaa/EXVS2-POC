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
    config_struct config;

    // config reading
    config.Windowed = reader.GetBoolean("config", "windowed", false);
    config.UseDirectInput = reader.GetBoolean("config", "usedirectinput", false);
    config.PcbId = reader.Get("config", "PcbId", "ABLN1110001");
    config.Serial = reader.Get("config", "serial", "284311110001");
    config.IpAddress = reader.Get("config", "IpAddress", "192.168.50.239");
    config.Gateway = reader.Get("config", "Gateway", "192.168.50.1");
    config.SubnetMask = reader.Get("config", "SubnetMask", "255.255.255.0");
    config.PrimaryDNS = reader.Get("config", "DNS", "8.8.8.8");
    config.TenpoRouter = reader.Get("config", "TenpoRouter", "192.168.50.1");
    config.AuthServerIp = reader.Get("config", "AuthIP", "127.0.0.1");
    config.ServerAddress = reader.Get("config", "Server", "127.0.0.1");
    

    // key bind config reading
    jvs_key_bind key_bind;
    std::string keyMapPlaceholder;

    keyMapPlaceholder = reader.Get("keybind", "Test", "T");
    key_bind.Test = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Start", "O");
    key_bind.Start = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Service", "S");
    key_bind.Service = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Up", "Up");
    key_bind.Up = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Left", "Left");
    key_bind.Left = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Down", "Down");
    key_bind.Down = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Right", "Right");
    key_bind.Right = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Button1", "Z");
    key_bind.Button1 = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Button2", "X");
    key_bind.Button2 = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Button3", "C");
    key_bind.Button3 = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "Button4", "V");
    key_bind.Button4 = findKeyByValue(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "ArcadeButton1", "1");
    key_bind.ArcadeButton1 = std::stoi(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "ArcadeButton2", "2");
    key_bind.ArcadeButton2 = std::stoi(keyMapPlaceholder);
    
    keyMapPlaceholder = reader.Get("keybind", "ArcadeButton3", "3");
    key_bind.ArcadeButton3 = std::stoi(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "ArcadeButton4", "4");
    key_bind.ArcadeButton4 = std::stoi(keyMapPlaceholder);

    keyMapPlaceholder = reader.Get("keybind", "ArcadeStartButton", "5");
    key_bind.ArcadeStartButton = std::stoi(keyMapPlaceholder);

    config.KeyBind = key_bind;
    return config;
}

[[noreturn]]void InitThread(config_struct config)
{
    InitAmAuthEmu(config);
    OutputDebugStringA("AmAuth Init");
    for (;;){}
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    // Read config
    // todo: consolidate items from GameHooks.cpp
    INIReader reader("config.ini");

    // todo: Give a proper default config
    config_struct config {};
    if (reader.ParseError() == 0)
    {
        config = ReadConfigs(reader);
    }

    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        {
            std::thread t(InitThread, config);
            t.detach();
            InitializeHooks();
            InitializeJvs(config);
            InitDXGIWindowHook(config);
        }
        break;
    case DLL_THREAD_ATTACH:
        break;
    case DLL_THREAD_DETACH:
        break;
    case DLL_PROCESS_DETACH:
        ExitAmAuthEmu();
        break;
    }
	
    return TRUE;
}
