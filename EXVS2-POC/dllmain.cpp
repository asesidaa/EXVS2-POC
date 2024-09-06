#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <shellapi.h>
#include <psapi.h>

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

#include "AmAuthEmu.h"
#include "AudioHooks.h"
#include "COMHooks.h"
#include "Configs.h"
#include "GameHooks.h"
#include "Input.h"
#include "JvsEmu.h"
#include "log.h"
#include "PatchTargets.h"
#include "SocketHooks.h"
#include "Version.h"
#include "VirtualKeyMapping.h"
#include "WindowedDxgi.h"

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
        switch (entryOffset)
        {
        case 0x4dad54:
            return VS2_400;
        case 0x51e9d4:
            return XBoost_450;
        default:
            fatal("failed to identify executable version: entrypoint = %" PRIx64, entryOffset);
        }
    }();
    return version;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        {
            std::filesystem::path basePath = GetBasePath();

            InitializeConfig();
            InitializeInput();

            MH_Initialize();
            InitializeSocketHooks();
            InitAmAuthEmu();
            InitializeHooks(std::move(basePath));
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
            const auto& TARGETS = VS2_XB(PATCH_TARGETS_VS2, PATCH_TARGETS_XBOOST);
            for (auto offset : TARGETS) {
              patch_targets.push_back(base + offset);
            }

            auto before = std::chrono::steady_clock::now();
            determinize::Determinize(std::move(patch_targets), base);
            auto after = std::chrono::steady_clock::now();

            info(
                "Patched %zu floating point approximations with deterministic "
                "implementations in %dms",
                TARGETS.size(),
                static_cast<int>((after - before) / 1.0ms));
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
