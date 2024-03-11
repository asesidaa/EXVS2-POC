// ReSharper disable CppClangTidyClangDiagnosticMicrosoftCast
#include "GameHooks.h"

#include <filesystem>
#include <string>
#include <windows.h>

#include <Xinput.h>

#include "injector.hpp"
#include "MinHook.h"
#include "log.h"
#include "INIReader.h"
#include "Configs.h"
#include "Version.h"

static constexpr auto BASE_ADDRESS = 0x140000000;
static HANDLE hConnection = (HANDLE)0x1337;
static std::filesystem::path g_storageDirectory;

HANDLE(__stdcall *CreateFileAOri)(LPCSTR lpFileName,
    DWORD dwDesiredAccess,
    DWORD dwShareMode,
    LPSECURITY_ATTRIBUTES lpSecurityAttributes,
    DWORD dwCreationDisposition,
    DWORD dwFlagsAndAttributes,
    HANDLE hTemplateFile);

HANDLE __stdcall CreateFileAHook(LPCSTR lpFileName,
    DWORD dwDesiredAccess,
    DWORD dwShareMode,
    LPSECURITY_ATTRIBUTES lpSecurityAttributes,
    DWORD dwCreationDisposition,
    DWORD dwFlagsAndAttributes,
    HANDLE hTemplateFile)
{
    const auto name = std::string_view{lpFileName};
    if (const auto target = "COM"; name.find(target) != std::string::npos)
    {
        debug("CreateFileA with COM name %s", name.data());
        return hConnection;
    }
    debug("CreateFileA with name %s", name.data());
    if (name.starts_with("G:") || name.starts_with("F:"))
    {
        std::filesystem::path tail(name.substr(3));
        std::filesystem::path p = g_storageDirectory / tail;

        debug("Redirect to %s", p.string().c_str());
        if (p.has_extension())
        {
            create_directories(p.parent_path());
        }
        else
        {
            create_directories(p);
        }
        
        //DebugBreak();
        return CreateFileAOri(p.string().data(),
            dwDesiredAccess,
            dwShareMode,
            lpSecurityAttributes,
            dwCreationDisposition,
            dwFlagsAndAttributes,
            hTemplateFile);
    }
    return CreateFileAOri(lpFileName,
        dwDesiredAccess,
        dwShareMode,
        lpSecurityAttributes,
        dwCreationDisposition,
        dwFlagsAndAttributes,
        hTemplateFile); 
}

HANDLE(__stdcall *CreateFileWOri)(LPCWSTR lpFileName,
    DWORD dwDesiredAccess,
    DWORD dwShareMode,
    LPSECURITY_ATTRIBUTES lpSecurityAttributes,
    DWORD dwCreationDisposition,
    DWORD dwFlagsAndAttributes,
    HANDLE hTemplateFile);
HANDLE __stdcall CreateFileWHook(LPCWSTR lpFileName,
    DWORD dwDesiredAccess,
    DWORD dwShareMode,
    LPSECURITY_ATTRIBUTES lpSecurityAttributes,
    DWORD dwCreationDisposition,
    DWORD dwFlagsAndAttributes,
    HANDLE hTemplateFile)
{
    const auto name = std::wstring_view{lpFileName};
    if (const auto target = L"COM"; name.find(target) != std::string::npos)
    {
        debug("CreateFileW with COM name %S", name.data());
        return hConnection;
    }

    debug("CreateFileW with name %S", name.data());
    return CreateFileWOri(lpFileName,
        dwDesiredAccess,
        dwShareMode,
        lpSecurityAttributes,
        dwCreationDisposition,
        dwFlagsAndAttributes,
        hTemplateFile); 
}

UINT (*GetDriveTypeAOri)(LPCSTR lpRootPathName);
UINT GetDriveTypeAHook(LPCSTR lpRootPathName)
{
    trace("Root Path Name: %s", lpRootPathName);
    if (const auto name = "E:"; std::string_view{lpRootPathName}.find(name) != std::string::npos)
    {
        trace("Hit E:");
        return 2;
    }
    return GetDriveTypeAOri(lpRootPathName);
}

BOOL (*PathFileExistsAOri)(LPCSTR pszPath);
BOOL PathFileExistsAHook(LPCSTR pszPath)
{
    trace("PathFileExistsAHook: %s", pszPath);
    return PathFileExistsAOri(pszPath);
}

static int64_t nbamUsbFinderInitialize()
{
    trace("nbamUsbFinderInitialize");
    return 0;
}

static int64_t nbamUsbFinderRelease()
{
    trace("nbamUsbFinderRelease");
    return 0;
}

static int64_t __fastcall nbamUsbFinderGetSerialNumber(int a1, char* a2)
{
    trace("nbamUsbFinderGetSerialNumber");
    strcpy_s (a2, 16, globalConfig.Serial.c_str());
    return 0;
}

char (*dev_menu_options_orig)(__int64 a1, int a2);

static char __fastcall dev_menu_options_hook(__int64 a1, int a2)
{
    // 0 = Dev Menu: Enable Performance Meter
    // 6 = Dev Menu: Enforce ALL.NET / Server connection after 1am JST
    // 27 = Dev Menu: Disable PCB 1 + 2 Checking in LM
    if(a2 != 27 && a2 != 6 && a2 != 0)
    {
        return dev_menu_options_orig(a1, a2);
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
    
    return globalConfig.EnableInGamePerformanceMeter ? '1' : dev_menu_options_orig(a1, a2);
}

// Stub functions for Disable XInput
static DWORD WINAPI XInputGetState_Detour(DWORD dwUserIndex, XINPUT_STATE *pState)
{
    return ERROR_DEVICE_NOT_CONNECTED;
}

namespace vs2
{
    // Bypass Content Router Check For Vanilla EXVS2
    static bool __fastcall hook_140652DD0(__int64 a1, int a2)
    {
        return true;
    }
}


void InitializeHooks(std::filesystem::path&& basePath)
{
    g_storageDirectory = std::move(basePath);

    MH_CreateHookApi(L"kernel32.dll", "CreateFileW", CreateFileWHook, reinterpret_cast<void**>(&CreateFileWOri));
    MH_CreateHookApi(L"kernel32.dll", "CreateFileA", CreateFileAHook, reinterpret_cast<void**>(&CreateFileAOri));
    MH_CreateHookApi(L"kernel32.dll", "GetDriveTypeA", GetDriveTypeAHook, reinterpret_cast<void**>(&GetDriveTypeAOri));
    
    MH_CreateHookApi(L"shlwapi.dll", "PathFileExistsA", PathFileExistsAHook, reinterpret_cast<void**>(&PathFileExistsAOri));

    MH_CreateHookApi(L"nbamUsbFinder.dll", "nbamUsbFinderInitialize", nbamUsbFinderInitialize, nullptr);
    MH_CreateHookApi(L"nbamUsbFinder.dll", "nbamUsbFinderRelease", nbamUsbFinderRelease, nullptr);
    MH_CreateHookApi(L"nbamUsbFinder.dll", "nbamUsbFinderGetSerialNumber", nbamUsbFinderGetSerialNumber, nullptr);

    auto exeBase = reinterpret_cast<uintptr_t>(GetModuleHandle(nullptr));
    
    // Disable content router ip check
    auto offset = VS2_XB(0x140652DD0, 0x14069CA90) - BASE_ADDRESS;

    // Disable content router ip check
    if (GetGameVersion() == VS2_400)
    {
        MH_CreateHook(reinterpret_cast<void**>(exeBase + offset), vs2::hook_140652DD0, NULL);  
    }
    else
    {
        // Write 31 C0 FF C0
        injector::WriteMemoryRaw(exeBase + offset, (void*)"\x31\xC0\xFF\xC0", 4, true);
        injector::MakeNOP(exeBase + offset + 4, 0x25 - 4, true);
    }

    // Client type hack
    offset = VS2_XB(0x14066DF3C, 0x1406BC45C) - BASE_ADDRESS;
    injector::MakeNOP(exeBase + offset, 6, true);
    offset = VS2_XB(0x14066DF47, 0x1406BC467) - BASE_ADDRESS;
    injector::MakeNOP(exeBase + offset, 14, true);
    injector::WriteMemory(exeBase + offset + 14, '\xB8', true);
    injector::WriteMemory(exeBase + offset + 15, globalConfig.Mode, true);
    injector::WriteMemory(exeBase + offset + 16, '\x00', true);
    injector::WriteMemory(exeBase + offset + 17, '\x00', true);

    // Adapter patches, disable adapter check when there are more than 2 adapters
    if (GetGameVersion() == XBoost_450)
    {
        offset = 0x1402EB957 - BASE_ADDRESS;
        injector::WriteMemory(exeBase + offset, '\xEB', true);
        offset = 0x1402EBA71-BASE_ADDRESS;
        injector::MakeNOP(exeBase + offset, 6, true);
        offset = 0x1402EBC5F - BASE_ADDRESS;
        injector::WriteMemory(exeBase + offset, '\xEB', true);
        offset = 0x1402EC101-BASE_ADDRESS;
        injector::MakeNOP(exeBase + offset, 2, true);
        offset = 0x1402EC1B2 - BASE_ADDRESS;
        injector::WriteMemory(exeBase + offset, '\xEB', true);
        offset = 0x1402EC321-BASE_ADDRESS;
        injector::MakeNOP(exeBase + offset, 2, true);
        offset = 0x1402EC3B4 - BASE_ADDRESS;
        injector::WriteMemory(exeBase + offset, '\xEB', true);
    }
    else if (GetGameVersion() == VS2_400)
    {
        offset = 0x1402E59F7 - BASE_ADDRESS;
        injector::WriteMemory(exeBase + offset, '\xEB', true);
    }
    
    // A Hook to handle Performance Meter / ALL.NET Server Connection after 1AM / LM PCB 1 + 2 Checking
    offset = VS2_XB(0x14064C320, 0x140695E60) - BASE_ADDRESS;
    MH_CreateHook(reinterpret_cast<void**>(exeBase + offset), dev_menu_options_hook, reinterpret_cast<void**>(&dev_menu_options_orig));
    
    // Disable XINPUT in LM
    if(globalConfig.Mode == 2 || globalConfig.Mode == 4)
    {
        MH_CreateHookApi(L"XINPUT9_1_0.dll", "XInputGetState", XInputGetState_Detour, nullptr);
    }
}
