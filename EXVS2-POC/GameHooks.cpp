// ReSharper disable CppClangTidyClangDiagnosticMicrosoftCast
#include "GameHooks.h"

#include <string>
#include <windows.h>

#include "injector.hpp"
#include "MinHook.h"
#include "log.h"
#include "INIReader.h"

static uint8_t gClientMode = 2;
static std::string gSerial = "284311110001";
static const auto BASE_ADDRESS = 0x140000000;
static HANDLE hConnection = (HANDLE)0x1337;

static HWND (WINAPI* CreateWindowExWOri)(DWORD dwExStyle, LPCWSTR lpClassName, LPCWSTR lpWindowName, DWORD dwStyle, int X, int Y, int nWidth, int nHeight, HWND hWndParent, HMENU hMenu, HINSTANCE hInstance, LPVOID lpParam);
static HWND WINAPI CreateWindowExWHook(DWORD dwExStyle, LPCWSTR lpClassName, LPCWSTR lpWindowName, DWORD dwStyle, int X, int Y, int nWidth, int nHeight, HWND hWndParent, HMENU hMenu, HINSTANCE hInstance, LPVOID lpParam)
{
    if (nWidth > 0 && nHeight > 0)
    {
        dwStyle = WS_VISIBLE | WS_POPUP | WS_CAPTION | WS_SYSMENU | WS_MINIMIZEBOX;
        X = (GetSystemMetrics(SM_CXSCREEN) - nWidth) / 2;
        Y = (GetSystemMetrics(SM_CYSCREEN) - nHeight) / 2;
        
        // lpWindowName = L"POC";
    }

    return CreateWindowExWOri(dwExStyle, lpClassName, lpWindowName, dwStyle, X, Y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);
}

static BOOL(WINAPI* ShowCursorOri)(BOOL bShow);
static BOOL WINAPI ShowCursorHook(BOOL bShow)
{
    return ShowCursorOri(true);
}

static BOOL(WINAPI* MoveWindowOri)(HWND hWnd, int X, int Y, int nWidth, int  nHeight, BOOL bRepaint);
static BOOL WINAPI MoveWindowHook(HWND hWnd, int X, int Y, int nWidth, int  nHeight, BOOL bRepaint)
{
    if (nWidth > 0 && nHeight > 0)
    {
        X = (GetSystemMetrics(SM_CXSCREEN) - nWidth) / 2;
        Y = (GetSystemMetrics(SM_CYSCREEN) - nHeight) / 2;
        nWidth = 1920;
        nHeight = 1080;
    }
    
    return MoveWindowOri(hWnd, X, Y, nWidth, nHeight, bRepaint);
}

static BOOL(WINAPI* SetWindowPosOri)(HWND hWnd, HWND hWndInsertAfter, int  X, int  Y, int  cx, int  cy, UINT uFlags);
static BOOL WINAPI SetWindowPosHook(HWND hWnd, HWND hWndInsertAfter, int  X, int  Y, int  cx, int  cy, UINT uFlags)
{
    if (cx > 0 && cy > 0)
    {
        return SetWindowPosOri(hWnd, HWND_TOP, X, Y, cx, cy, uFlags);
    }

    return SetWindowPosOri(hWnd, hWndInsertAfter, X, Y, cx, cy, uFlags);
}

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
        log("CreateFileA with COM name %s", name.data());
        return hConnection;
    }
    log("CreateFileA with name %s", name.data());
    if (name.starts_with("G:") || name.starts_with("F:"))
    {
        //DebugBreak();
        return CreateFileAOri(name.substr(3).data(),
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
        log("CreateFileW with COM name %s", name.data());
        return hConnection;
    }

    log("CreateFileW with name %S", name.data());
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
    log("Root Path Name: %s", lpRootPathName);
    if (const auto name = "E:"; std::string_view{lpRootPathName}.find(name) != std::string::npos)
    {
        log("Hit E:");
        return 2;
    }
    return GetDriveTypeAOri(lpRootPathName);
}

BOOL (*PathFileExistsAOri)(LPCSTR pszPath);
BOOL PathFileExistsAHook(LPCSTR pszPath)
{
    log("PathFileExistsAHook: %s", pszPath);
    return PathFileExistsAOri(pszPath);
}

static int64_t nbamUsbFinderInitialize()
{
    log("nbamUsbFinderInitialize");
    return 0;
}

static int64_t nbamUsbFinderRelease()
{
    log("nbamUsbFinderRelease");
    return 0;
}

static int64_t __fastcall nbamUsbFinderGetSerialNumber(int a1, char* a2)
{
    log("nbamUsbFinderGetSerialNumber");
    strcpy_s (a2, 16, gSerial.c_str());
    return 0;
}


void InitializeHooks()
{
    MH_Initialize();

    MH_CreateHookApi(L"user32.dll", "CreateWindowExW", CreateWindowExWHook,
                     reinterpret_cast<void**>(&CreateWindowExWOri));
    // MH_CreateHookApi(L"user32.dll", "ShowCursor", ShowCursorHook, reinterpret_cast<void**>(&ShowCursorOri));
    // MH_CreateHookApi(L"user32.dll", "MoveWindow", MoveWindowHook, reinterpret_cast<void**>(&MoveWindowOri));
    MH_CreateHookApi(L"user32.dll", "SetWindowPos", SetWindowPosHook, reinterpret_cast<void**>(&SetWindowPosOri));

    MH_CreateHookApi(L"kernel32.dll", "CreateFileW", CreateFileWHook, reinterpret_cast<void**>(&CreateFileWOri));
    MH_CreateHookApi(L"kernel32.dll", "CreateFileA", CreateFileAHook, reinterpret_cast<void**>(&CreateFileAOri));
    MH_CreateHookApi(L"kernel32.dll", "GetDriveTypeA", GetDriveTypeAHook, reinterpret_cast<void**>(&GetDriveTypeAOri));
    
    MH_CreateHookApi(L"shlwapi.dll", "PathFileExistsA", PathFileExistsAHook, reinterpret_cast<void**>(&PathFileExistsAOri));

    MH_CreateHookApi(L"nbamUsbFinder.dll", "nbamUsbFinderInitialize", nbamUsbFinderInitialize, nullptr);
    MH_CreateHookApi(L"nbamUsbFinder.dll", "nbamUsbFinderRelease", nbamUsbFinderRelease, nullptr);
    MH_CreateHookApi(L"nbamUsbFinder.dll", "nbamUsbFinderGetSerialNumber", nbamUsbFinderGetSerialNumber, nullptr);

    MH_EnableHook(MH_ALL_HOOKS);

    // Create the 25 folder first if it doesn't exist
    CreateDirectoryA("25" , nullptr);
    
    INIReader reader("config.ini");
    if (reader.ParseError() == 0)
    {
        gClientMode = reader.GetInteger("config", "mode", 2);
        gSerial = reader.Get("config", "serial", "284311110001");
    }

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
    injector::WriteMemory(exeBase + offset + 15, gClientMode, true);
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
}
