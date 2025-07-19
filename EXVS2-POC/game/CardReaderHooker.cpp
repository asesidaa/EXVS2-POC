#include "CardReaderHooker.h"

#include <windows.h>

#include "MinHook.h"
#include "../Configs.h"
#include "../log.h"

static HINSTANCE bngrwOrig = LoadLibrary(TEXT("bngrw_orig"));
__int64 (*card_reader_post_process_orig)(__int64 a1, __int64 a2, int a3, char* a4, __int64 a5);

std::string convertToString(char* a, int size)
{
    int i;
    std::string s = "";
    for (i = 0; i < size; i++) {
        s = s + a[i];
    }
    return s;
}

__int64 card_reader_post_process(__int64 a1, __int64 a2, int a3, char* a4, __int64 a5)
{
    if (!globalConfig.UseRealCardReader)
    {
        return card_reader_post_process_orig(a1, a2, a3, a4, a5);
    }

    if (bngrwOrig == nullptr)
    {
        return card_reader_post_process_orig(a1, a2, a3, a4, a5);
    }

    char chipIdBuff[33];
    memcpy(chipIdBuff, &a4[44], 32);

    char accessCodeBuff[21];
    memcpy(accessCodeBuff, &a4[80], 20);
    
    info("[Real BanaPass] Chip ID: '%s', Access Code: '%s'", convertToString(chipIdBuff, 32).c_str(), convertToString(accessCodeBuff, 20).c_str());
    return card_reader_post_process_orig(a1, a2, a3, a4, a5);
}

void hook_card_reader_post_process(GameVersion game_version, uintptr_t exe_base_pointer, const long long base_address)
{
    auto card_reader_post_process_offset = VS2_XB_OB(0x140109C10, 0x14010F7D0, 0x140112410) - base_address;
    
    MH_CreateHook(
        reinterpret_cast<void**>(exe_base_pointer + card_reader_post_process_offset),
        card_reader_post_process,
        reinterpret_cast<void**>(&card_reader_post_process_orig)
    );
}