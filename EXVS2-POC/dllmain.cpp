#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <psapi.h>
#include <shellapi.h>

#include <stdio.h>
#include <io.h>
#include <fcntl.h>

#include <inttypes.h>

#include <algorithm>
#include <chrono>
#include <filesystem>
#include <string>
#include <string_view>
#include <thread>

#include <determinize/determinize.h>
#include <MinHook.h>

#include "AMActivatorHook.h"
#include "AmAuthEmu.h"
#include "AudioHooks.h"
#include "COMHooks.h"
#include "Configs.h"
#include "GameFileSystemHook.h"
#include "GameHooks.h"
#include "Input.h"
#include "JvsEmu.h"
#include "SocketHooks.h"
#include "Version.h"
#include "VirtualKeyMapping.h"
#include "WindowedDxgi.h"
#include "log.h"
#include "NbamUsbFinderHook.h"
#include "amauth/v3/AmAuthEmuV3.h"
#include "cpu/over/ObV27.h"
#include "cpu/vs2/Vs2V29.h"
#include "cpu/xb/XbV27.h"
#include "file/RentalModeFileCleaner.h"

using namespace std::chrono_literals;

std::filesystem::path GetBasePath()
{
    int argc;
    LPWSTR* argv = CommandLineToArgvW(GetCommandLineW(), &argc);
    switch (argc)
    {
    case 0:
        fatal("Empty commandline");
    case 1:
        return std::filesystem::current_path();
    case 2:
        return std::filesystem::path(argv[1]);
    default:
        fatal("Invalid commandline (expected only a path, argc = %d)", argc);
    }
}

GameVersion GetGameVersion()
{
    static auto version = []() {
        HMODULE executable = nullptr;
        if (!GetModuleHandleExA(0, nullptr, &executable))
            fatal("failed to get executable module");
        MODULEINFO moduleInfo;
        if (!GetModuleInformation(GetCurrentProcess(), executable, &moduleInfo, sizeof(moduleInfo)))
            fatal("failed to get executable module info");

        uint64_t loadAddress = (uint64_t)moduleInfo.lpBaseOfDll;
        uint64_t entryPoint = (uint64_t)moduleInfo.EntryPoint;
        uint64_t entryOffset = entryPoint - loadAddress;
        
        if(entryOffset == 0x4dad54)
        {
            return VS2_400;
        }

        if(entryOffset == 0x51e9d4)
        {
            return XBoost_450;
        }

        if(entryOffset == 0x53c9ac)
        {
            return Overboost_480;
        }

        fatal("failed to identify executable version: entrypoint = %" PRIx64, entryOffset);
    }();
    return version;
}


BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        {
            std::filesystem::path basePath = GetBasePath();
            std::filesystem::path rentalBasePath = GetBasePath();

            GameVersion game_version = GetGameVersion();
            
            InitializeConfig();
            InitializeInput();

            MH_Initialize();
            InitializeSocketHooks();
            InitializeInputHooks();
            
            if (game_version == Overboost_480)
            {
                InitAmAuthEmuV3();
                InitializeAmActivatorHooks();
            }
            else
            {
                InitAmAuthEmu();
            }
            
            clean_up_rental_mode_file(std::move(rentalBasePath), game_version);
            InitializeGameFileSystemHooks(std::move(basePath));
            
            InitializeNbamUsbFinderHooks();
            InitializeHooks(game_version);
            InitializeJvs();
            InitDXGIWindowHook();

            if (!globalConfig.Audio.DisableHook)
            {
                InitializeAudioHooks();
            }
            InitializeCOMHooks();
            MH_EnableHook(MH_ALL_HOOKS);
            
            auto base = reinterpret_cast<char*>(GetModuleHandle(nullptr));
            std::vector<void*> patch_targets;
            const auto& TARGETS = VS2_XB_OB(PATCH_TARGETS_VS2_V29, PATCH_TARGETS_XBOOST_V27, PATCH_TARGETS_OVERBOOST_V27);
            for (auto offset : TARGETS) {
                patch_targets.push_back(base + offset);
            }
            
            auto before = std::chrono::steady_clock::now();
            determinize::Determinize(std::move(patch_targets), base);
            auto after = std::chrono::steady_clock::now();

            info("Patched %zu floating point approximations with deterministic "
                 "implementations in %dms\n",
                 TARGETS.size(), static_cast<int>((after - before) / 1.0ms));
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
