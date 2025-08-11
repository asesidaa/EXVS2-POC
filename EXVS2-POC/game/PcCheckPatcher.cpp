// ReSharper disable CppClangTidyClangDiagnosticMicrosoftCast
#include "PcCheckPatcher.h"

#include "MinHook.h"
#include "../Configs.h"
#include "../log.h"

static HWND(WINAPI* g_origGetRawInputDeviceInfoW)(HANDLE hDevice, UINT uiCommand, LPVOID pData, PUINT pcbSize);

static HWND WINAPI GetRawInputDeviceInfoWHook(HANDLE hDevice, UINT uiCommand, LPVOID pData, PUINT pcbSize)
{
    auto result = g_origGetRawInputDeviceInfoW(hDevice, uiCommand, pData, pcbSize);
    
    RID_DEVICE_INFO* deviceInfo = reinterpret_cast<RID_DEVICE_INFO*>(pData);

    if (deviceInfo == nullptr)
    {
        return result;
    }

    if (deviceInfo->dwType != RIM_TYPEKEYBOARD)
    {
        return result;
    }

    deviceInfo->keyboard.dwNumberOfKeysTotal = 0;
    
    return result;
}

__int64 (*hardware_detection_function_orig)();

__int64 hardware_detection_function_hook()
{
    auto result = hardware_detection_function_orig();
    info("Hardware Calculation Function Result = %d, forced to 0", result);
    return 0;
}

void patch_pc_check(GameVersion game_version, uintptr_t exe_base_pointer, const long long base_address)
{
    if (game_version != Overboost_480)
    {
        return;
    }
    
    MH_CreateHookApi(L"user32.dll", "GetRawInputDeviceInfoW", GetRawInputDeviceInfoWHook, reinterpret_cast<void**>(&g_origGetRawInputDeviceInfoW));
    info("Disable Keyboard Checking...");
    
    auto hardware_detection_function_offset = 0x140452A60 - base_address;
    MH_CreateHook(reinterpret_cast<void**>(exe_base_pointer + hardware_detection_function_offset), hardware_detection_function_hook, reinterpret_cast<void**>(&hardware_detection_function_orig));
    info("Disable Hardware Checking...");
}