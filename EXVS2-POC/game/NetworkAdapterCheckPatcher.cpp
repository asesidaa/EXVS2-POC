#include "NetworkAdapterCheckPatcher.h"
#include <windows.h>
#include "injector.hpp"

void patch_network_adapter_check(GameVersion game_version, uintptr_t exe_base_pointer, const long long base_address)
{
    if(game_version == VS2_400)
    {
        auto vs2_adapter_offset = 0x1402E59F7 - base_address;
        injector::WriteMemory(exe_base_pointer + vs2_adapter_offset, '\xEB', true);
        return;
    }

    auto adapter_offset_1 = XB_OB(0x1402EB957, 0x1402EEE57) - base_address;
    injector::WriteMemory(exe_base_pointer + adapter_offset_1, '\xEB', true);

    auto adapter_offset_2 = XB_OB(0x1402EBA71, 0x1402EEF71) - base_address;
    injector::MakeNOP(exe_base_pointer + adapter_offset_2, 6, true);
        
    auto adapter_offset_3 = XB_OB(0x1402EBC5F, 0x1402EF15F) - base_address;
    injector::WriteMemory(exe_base_pointer + adapter_offset_3, '\xEB', true);
        
    auto adapter_offset_4 = XB_OB(0x1402EC101, 0x1402EF601) - base_address;
    injector::MakeNOP(exe_base_pointer + adapter_offset_4, 2, true);
        
    auto adapter_offset_5 = XB_OB(0x1402EC1B2, 0x1402EF6B2) - base_address;
    injector::WriteMemory(exe_base_pointer + adapter_offset_5, '\xEB', true);
        
    auto adapter_offset_6 = XB_OB(0x1402EC321, 0x1402EF821) - base_address;
    injector::MakeNOP(exe_base_pointer + adapter_offset_6, 2, true);
        
    auto adapter_offset_7 = XB_OB(0x1402EC3B4, 0x1402EF8B4) - base_address;
    injector::WriteMemory(exe_base_pointer + adapter_offset_7, '\xEB', true);
}