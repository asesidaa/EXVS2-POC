// ReSharper disable CppClangTidyClangDiagnosticMicrosoftCast

#include "AMActivatorHook.h"
#include "MinHook.h"
#include "injector.hpp"
#include "log.h"
#include <string>

static constexpr auto BASE_ADDRESS = 0x180000000;

static void __fastcall AMActivator_Update_Hook(__int64 a1)
{
}

int (*AMActivator_GetOneTimeKeyLastStatusOri)(__int64 a1);

static int __fastcall AMActivator_GetOneTimeKeyLastStatus_Hook(__int64 a1)
{
    return 3;
}

static int __fastcall AMActivator_GetOneTimeKey_Hook(__int64 a1)
{
    return 23323300;
}

static uint64_t __fastcall AMActivator_GetOneTimeKeyExpiration_Hook(__int64 a1)
{
    // Year of 2099
    return 4070908800;
}

static int __fastcall AMActivator_GetSignatureLastStatus_Hook(__int64 a1)
{
    // 4 = Dongle Error
    // 11 = Unknown Error
    return 3;
}

static int __fastcall AMActivator_GetSignatureGeneration_Hook(__int64 a1)
{
    return 1;
}

char (*AMActivator_RequestSignature_Ori)(__int64 a1);

static char __fastcall AMActivator_RequestSignature_Hook(__int64 a1)
{
    info("AMActivator_RequestSignature");

    uint64_t timestamp = *(uint64_t*)(a1 + 344);

    std::string my_val = std::to_string(timestamp);
    info("%s", my_val.c_str());

    *(uint64_t*)(a1 + 344) = 4070908800;

    std::string my_val_2 = std::to_string(*(uint64_t*)(a1 + 344));
    info("%s", my_val_2.c_str());

    return AMActivator_RequestSignature_Ori(a1);
}

void InitializeAmActivatorHooks()
{
    MH_CreateHookApi(L"amactivator.dll", "AMActivator_Update", AMActivator_Update_Hook, nullptr);
    MH_CreateHookApi(L"amactivator.dll", "AMActivator_GetOneTimeKeyLastStatus",
                     AMActivator_GetOneTimeKeyLastStatus_Hook,
                     reinterpret_cast<void**>(&AMActivator_GetOneTimeKeyLastStatusOri));
    MH_CreateHookApi(L"amactivator.dll", "AMActivator_GetOneTimeKey", AMActivator_GetOneTimeKey_Hook, nullptr);
    MH_CreateHookApi(L"amactivator.dll", "AMActivator_GetOneTimeKeyExpiration",
                     AMActivator_GetOneTimeKeyExpiration_Hook, nullptr);
    MH_CreateHookApi(L"amactivator.dll", "AMActivator_GetSignatureLastStatus", AMActivator_GetSignatureLastStatus_Hook,
                     nullptr);
    MH_CreateHookApi(L"amactivator.dll", "AMActivator_GetSignatureGeneration", AMActivator_GetSignatureGeneration_Hook,
                     nullptr);

    MH_CreateHookApi(L"amactivator.dll", "AMActivator_RequestSignature", AMActivator_RequestSignature_Hook,
                     reinterpret_cast<void**>(&AMActivator_RequestSignature_Ori));
}
