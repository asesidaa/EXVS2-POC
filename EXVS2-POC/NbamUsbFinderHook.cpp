#include "NbamUsbFinderHook.h"

#include "Configs.h"
#include "log.h"
#include "MinHook.h"

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

void InitializeNbamUsbFinderHooks()
{
    MH_CreateHookApi(L"nbamUsbFinder.dll", "nbamUsbFinderInitialize", nbamUsbFinderInitialize, nullptr);
    MH_CreateHookApi(L"nbamUsbFinder.dll", "nbamUsbFinderRelease", nbamUsbFinderRelease, nullptr);
    MH_CreateHookApi(L"nbamUsbFinder.dll", "nbamUsbFinderGetSerialNumber", nbamUsbFinderGetSerialNumber, nullptr);
}