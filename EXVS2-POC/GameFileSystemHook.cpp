#include "GameFileSystemHook.h"

#include <filesystem>
#include <windows.h>

#include "Configs.h"
#include "log.h"
#include "MinHook.h"

static std::filesystem::path g_storageDirectory;
static HANDLE hConnection = (HANDLE)0x1337;
static std::string normalized_storage_directory;
static std::string normalized_shortcut_directory;

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

        if (name == "COM3")
        {
            if(globalConfig.UseIoBoard == false)
            {
                return hConnection;
            }

            info("Detected real JVS");

            return CreateFileAOri(lpFileName,
                dwDesiredAccess,
                dwShareMode,
                lpSecurityAttributes,
                dwCreationDisposition,
                dwFlagsAndAttributes,
                hTemplateFile);
        }

        if (globalConfig.UseRealCardReader == false)
        {
            return hConnection;
        }
        
        info("%s Detected with Real Card Reader Enabled, Will use Real Card Reader", globalConfig.CardReaderComPort.c_str());
        return CreateFileAOri(globalConfig.CardReaderComPort.c_str(),
            dwDesiredAccess,
            dwShareMode,
            lpSecurityAttributes,
            dwCreationDisposition,
            dwFlagsAndAttributes,
            hTemplateFile);
    }
    debug("CreateFileA with name %s", name.data());

    if (name.starts_with(normalized_storage_directory) || name.starts_with(normalized_shortcut_directory))
    {
        // info("%s, Do Nothing", name.data());
        return CreateFileAOri(lpFileName,
            dwDesiredAccess,
            dwShareMode,
            lpSecurityAttributes,
            dwCreationDisposition,
            dwFlagsAndAttributes,
            hTemplateFile);
    }
    
    auto normalizedLoadingFile = std::string(name.data());
    std::ranges::replace(normalizedLoadingFile.begin(), normalizedLoadingFile.end(), '\\', '/');

    if(normalizedLoadingFile.starts_with(normalized_storage_directory) || normalizedLoadingFile.starts_with(normalized_shortcut_directory))
    {
        return CreateFileAOri(lpFileName,
            dwDesiredAccess,
            dwShareMode,
            lpSecurityAttributes,
            dwCreationDisposition,
            dwFlagsAndAttributes,
            hTemplateFile);
    }
    
    debug("Normalized Directory name %s", normalized_storage_directory.c_str());
    debug("Normalized Directory 2 name %s", normalized_shortcut_directory.c_str());
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

void InitializeGameFileSystemHooks(std::filesystem::path&& basePath)
{
    g_storageDirectory = std::move(basePath);
    
    normalized_storage_directory = g_storageDirectory.string();
    std::ranges::replace(normalized_storage_directory.begin(), normalized_storage_directory.end(), '\\', '/');

    normalized_shortcut_directory = std::filesystem::current_path().string();
    std::ranges::replace(normalized_shortcut_directory.begin(), normalized_shortcut_directory.end(), '\\', '/');

    MH_CreateHookApi(L"kernel32.dll", "CreateFileW", CreateFileWHook, reinterpret_cast<void**>(&CreateFileWOri));
    MH_CreateHookApi(L"kernel32.dll", "CreateFileA", CreateFileAHook, reinterpret_cast<void**>(&CreateFileAOri));
    MH_CreateHookApi(L"kernel32.dll", "GetDriveTypeA", GetDriveTypeAHook, reinterpret_cast<void**>(&GetDriveTypeAOri));
    
    MH_CreateHookApi(L"shlwapi.dll", "PathFileExistsA", PathFileExistsAHook, reinterpret_cast<void**>(&PathFileExistsAOri));
}