#include "OpeningScreenSkipper.h"

#include <windows.h>

#include "injector.hpp"
#include "../Configs.h"

void skip_opening_screen(GameVersion game_version, uintptr_t exe_base_pointer, const long long base_address)
{
    auto opening_screen_bypass_offset = VS2_XB_OB(0x1408AEC37, 0x140926497, 0x140990AC7) - base_address;

    if(globalConfig.OpeningScreenSkip == "None")
    {
        // Do Nothing here
        return;
    }

    if(globalConfig.OpeningScreenSkip == "SkipReminder")
    {
        injector::WriteMemoryRaw(exe_base_pointer + opening_screen_bypass_offset + 0x5, (void*)"\x74\x52", 2, true); // 67 -> 52
        injector::WriteMemoryRaw(exe_base_pointer + opening_screen_bypass_offset + 0xA, (void*)"\x74\x4D", 2, true); // 4D -> 4D
        return;
    }

    if(globalConfig.OpeningScreenSkip == "SkipBrand")
    {
        injector::WriteMemoryRaw(exe_base_pointer + opening_screen_bypass_offset + 0x5, (void*)"\x74\x3D", 2, true); // 67 -> 3D
        injector::WriteMemoryRaw(exe_base_pointer + opening_screen_bypass_offset + 0xA, (void*)"\x74\x38", 2, true); // 4D -> 38
        return;
    }

    injector::WriteMemoryRaw(exe_base_pointer + opening_screen_bypass_offset + 0x5, (void*)"\x74\x28", 2, true); // 67 -> 28
    injector::WriteMemoryRaw(exe_base_pointer + opening_screen_bypass_offset + 0xA, (void*)"\x74\x23", 2, true); // 4D -> 38
}
