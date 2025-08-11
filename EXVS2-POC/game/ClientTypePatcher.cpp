#include "ClientTypePatcher.h"

#include <windows.h>
#include "injector.hpp"
#include "../Configs.h"

void patch_client_type(GameVersion game_version, uintptr_t exe_base_pointer, const long long base_address)
{
    auto client_type_offset_1 = VS2_XB_OB(0x14066DF3C, 0x1406BC45C, 0x1406DF9CC) - base_address;
    injector::MakeNOP(exe_base_pointer + client_type_offset_1, 6, true);
    
    auto client_type_offset_2 = VS2_XB_OB(0x14066DF47, 0x1406BC467, 0x1406DF9D7) - base_address;
    injector::MakeNOP(exe_base_pointer + client_type_offset_2, 14, true);
    injector::WriteMemory(exe_base_pointer + client_type_offset_2 + 14, '\xB8', true);
    injector::WriteMemory(exe_base_pointer + client_type_offset_2 + 15, globalConfig.Mode, true);
    injector::WriteMemory(exe_base_pointer + client_type_offset_2 + 16, '\x00', true);
    injector::WriteMemory(exe_base_pointer + client_type_offset_2 + 17, '\x00', true);
}
