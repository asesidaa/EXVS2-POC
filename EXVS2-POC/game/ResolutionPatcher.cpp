#include "ResolutionPatcher.h"

#include <windows.h>

#include "injector.hpp"
#include "../Configs.h"
#include "../DisplayConstants.h"
#include "../log.h"

void patch_resolution(GameVersion game_version, uintptr_t exe_base_pointer, const long long base_address)
{
    if(globalConfig.Display.Resolution == "1080p")
    {
        return;
    }

    info("Perform Resolution Patch at %s", globalConfig.Display.Resolution.c_str());

    int targetWidth = FullHdWidth;
    int targetHeight = FullHdHeight;
    
    if(globalConfig.Display.Resolution == "144p")
    {
        targetWidth = YouTubeLowestWidth;
        targetHeight = YouTubeLowestHeight;
    }

    if(globalConfig.Display.Resolution == "240p")
    {
        targetWidth = NtscWidth;
        targetHeight = NtscHeight;
    }

    if(globalConfig.Display.Resolution == "480p")
    {
        targetWidth = DvdWideWidth;
        targetHeight = DvdWideHeight;
    }

    if(globalConfig.Display.Resolution == "720p")
    {
        targetWidth = HdWidth;
        targetHeight = HdHeight;
    }

    if(globalConfig.Display.Resolution == "900p")
    {
        targetWidth = NineHundredPWidth;
        targetHeight = NineHundredPHeight;
    }

    if(globalConfig.Display.Resolution == "2k")
    {
        targetWidth = QhdWidth;
        targetHeight = QhdHeight;
    }

    if(globalConfig.Display.Resolution == "4k")
    {
        targetWidth = UhdWidth;
        targetHeight = UhdHeight;
    }

    if(globalConfig.Display.Resolution == "8k")
    {
        targetWidth = Uhd8KWidth;
        targetHeight = Uhd8KHeight;
    }

    int windowWidth = targetWidth;
    int windowHeight = targetHeight;

    if(windowWidth >= FullHdWidth)
    {
        windowWidth = FullHdWidth;
        windowHeight = FullHdHeight;
    }
    
    auto full_hd_window_offset = VS2_XB_OB(0x140106232, 0x140109912, 0x14010A802) - base_address;
    injector::WriteMemory(exe_base_pointer + full_hd_window_offset, windowWidth, true);
    injector::WriteMemory(exe_base_pointer + full_hd_window_offset + 0xA, windowHeight, true);
    
    auto resolution_check_offset = VS2_XB_OB(0x14068DF60, 0x1406DFE80, 0x14070A510) - base_address;
    injector::WriteMemory(exe_base_pointer + resolution_check_offset, targetWidth, true);
    injector::WriteMemory(exe_base_pointer + resolution_check_offset + 0x9, targetHeight, true);
    
    auto resolution_patch_1_offset = VS2_XB_OB(0x140106246, 0x140109926, 0x14010A816) - base_address;
    injector::WriteMemory(exe_base_pointer + resolution_patch_1_offset, targetWidth, true);
    injector::WriteMemory(exe_base_pointer + resolution_patch_1_offset + 0xA, targetHeight, true);
    
    auto resolution_patch_2_offset = VS2_XB_OB(0x14010625A, 0x14010993A, 0x14010A82A) - base_address;
    injector::WriteMemory(exe_base_pointer + resolution_patch_2_offset, targetWidth, true);
    injector::WriteMemory(exe_base_pointer + resolution_patch_2_offset + 0xA, targetHeight, true);
}