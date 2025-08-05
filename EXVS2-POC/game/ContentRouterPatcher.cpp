// ReSharper disable CppClangTidyClangDiagnosticMicrosoftCast
#include "ContentRouterPatcher.h"

#include <windows.h>

#include "injector.hpp"
#include "MinHook.h"

namespace vs2_hook
{
    static bool __fastcall is_valid_vs2_content_router(__int64 a1, int a2)
    {
        return true;
    }
}

void patch_content_router(GameVersion game_version, uintptr_t exe_base_pointer, const long long base_address)
{
    if(game_version == VS2_400)
    {
        auto vs2_content_router_offset = 0x140652DD0 - base_address;
        MH_CreateHook(
            reinterpret_cast<void**>(exe_base_pointer + vs2_content_router_offset),
            vs2_hook::is_valid_vs2_content_router,
            NULL
        );
        return;
    }
    
    auto content_router_offset = XB_OB(0x14069CA90, 0x1406B5BB0) - base_address;
    injector::WriteMemoryRaw(exe_base_pointer + content_router_offset, (void*)"\x31\xC0\xFF\xC0", 4, true);
    injector::MakeNOP(exe_base_pointer + content_router_offset + 4, 0x25 - 4, true);
}