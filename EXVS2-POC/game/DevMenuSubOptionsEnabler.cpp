// ReSharper disable CppClangTidyClangDiagnosticMicrosoftCast
#include "DevMenuSubOptionsEnabler.h"

#include <windows.h>

#include "MinHook.h"
#include "../Configs.h"
#include "../Version.h"

char (*dev_menu_options_orig)(__int64 a1, int a2);

char get_quick_skin_change_enabled_status(long long a1, int a2)
{
    if(GetGameVersion() != Overboost_480)
    {
        return dev_menu_options_orig(a1, a2);
    }
        
    if(globalConfig.Mode == 2 || globalConfig.Mode == 4)
    {
        return dev_menu_options_orig(a1, a2);
    }

    return globalConfig.EnableQuickSkinChange ? '1' : dev_menu_options_orig(a1, a2);
}

static char __fastcall dev_menu_options_hook(__int64 a1, int a2)
{
    // 0 = Dev Menu: Enable Performance Meter
    // 6 = Dev Menu: Enforce ALL.NET / Server connection after 1am JST
    // 27 = Dev Menu: Disable PCB 1 + 2 Checking in LM
    // 37 = Dev Menu (OB): Enable Quick Skin Change
    if(a2 == 0)
    {
        return globalConfig.EnableInGamePerformanceMeter ? '1' : dev_menu_options_orig(a1, a2);
    }

    if(a2 == 6)
    {
        return '1';
    }

    if(a2 == 27)
    {
        if(globalConfig.Mode == 1 || globalConfig.Mode == 3)
        {
            return dev_menu_options_orig(a1, a2);
        }

        return '1';
    }

    if(a2 == 37)
    {
        return get_quick_skin_change_enabled_status(a1, a2);
    }

    return dev_menu_options_orig(a1, a2);
}

void enable_dev_menu_sub_options(GameVersion game_version, uintptr_t exe_base_pointer, const long long base_address)
{
    auto dev_menu_options = VS2_XB_OB(0x14064C320, 0x140695E60, 0x1406BBEF0) - base_address;
    MH_CreateHook(reinterpret_cast<void**>(exe_base_pointer + dev_menu_options), dev_menu_options_hook, reinterpret_cast<void**>(&dev_menu_options_orig));
}