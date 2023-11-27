#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <shellapi.h>

#include <algorithm>
#include <chrono>
#include <filesystem>
#include <string>
#include <string_view>
#include <thread>

#include <determinize/determinize.h>
#include <MinHook.h>

#include "AmAuthEmu.h"
#include "Configs.h"
#include "GameHooks.h"
#include "Input.h"
#include "JvsEmu.h"
#include "log.h"
#include "PatchTargets.h"
#include "SocketHooks.h"
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
            MH_EnableHook(MH_ALL_HOOKS);

            auto base = reinterpret_cast<char*>(GetModuleHandle(nullptr));
            std::vector<void*> patch_targets;
            for (auto offset : PATCH_TARGETS) {
              patch_targets.push_back(base + offset);
            }

            auto before = std::chrono::steady_clock::now();
            determinize::Determinize(std::move(patch_targets), base);
            auto after = std::chrono::steady_clock::now();

            printf(
                "Patched %zu floating point approximations with deterministic "
                "implementations in %dms\n",
                PATCH_TARGETS.size(),
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
