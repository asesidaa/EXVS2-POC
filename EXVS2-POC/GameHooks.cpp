// ReSharper disable CppClangTidyClangDiagnosticMicrosoftCast
#include "GameHooks.h"

#include <filesystem>
#include <string>
#include <windows.h>

#include "injector.hpp"
#include "MinHook.h"
#include "log.h"
#include "INIReader.h"
#include "Configs.h"

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
    auto offset = 0x14069CA90 - BASE_ADDRESS;
    // Write 31 C0 FF C0
    injector::WriteMemoryRaw(exeBase + offset, (void*)"\x31\xC0\xFF\xC0", 4, true);
    injector::MakeNOP(exeBase + offset + 4, 0x25 - 4, true);

    // Client type hack
    offset = 0x1406BC45C - BASE_ADDRESS;
    injector::MakeNOP(exeBase + offset, 6, true);
    offset = 0x1406BC467 - BASE_ADDRESS;
    injector::MakeNOP(exeBase + offset, 14, true);
    injector::WriteMemory(exeBase + offset + 14, '\xB8', true);
    injector::WriteMemory(exeBase + offset + 15, globalConfig.Mode, true);
    injector::WriteMemory(exeBase + offset + 16, '\x00', true);
    injector::WriteMemory(exeBase + offset + 17, '\x00', true);

    // Adapter patches, disable adapter check when there are more than 2 adapters
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

    // Clock patches, disable close time disabling card scanning feature.
    // If close time is set between 19:00 to 26:00, the red "card reading is unavaliable now" text is shown:
    offset = 0x140695BA2 - BASE_ADDRESS;
    injector::WriteMemoryRaw(exeBase + offset, (void*)"\x41\xb8\x00\x00\x00\x00\xeb\x13\x8b\xc3\x41\xc7\xc0\x00\x00\x00\x00\x90\xeb\x07\x41\xc7\xc0\x00\x00\x00\x00", 27, true);
    // This is the actual function to check, redirect the return function to make card reading possible regardless of closing time.
    offset = 0x1406B752E - BASE_ADDRESS;
    injector::WriteMemory(exeBase + offset, '\xEB', true);
}
