#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <string>

#include "GameHooks.h"
#include "JvsEmu.h"
#include "WindowedDxgi.h"
#include "INIReader.h"
#include "VirtualKeyMapping.h"

struct config_struct {
    jvs_key_bind key_bind;
    bool Windowed;
} config;

config_struct ReadConfigs(INIReader reader) {
    config_struct config;

    config.Windowed = reader.GetBoolean("config", "windowed", false);

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

    config.key_bind = key_bind;
    return config;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    // Read config
    // todo: consolidate items from GameHooks.cpp
    INIReader reader("config.ini");

    config_struct config;
    if (reader.ParseError() == 0)
    {
        config = ReadConfigs(reader);
    }

    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        InitializeHooks();
        InitializeJvs(config.key_bind);
        InitDXGIWindowHook(config.Windowed);
        break;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
	
    return TRUE;
}
