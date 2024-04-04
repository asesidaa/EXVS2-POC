#include "banapass.h"

#include <chrono>
#include <filesystem>
#include <string>
#include <thread>

#include <Windows.h>
#include <joystickapi.h>

#include "Configs.h"
#include "INIReader.h"
#include "Input.h"
#include "log.h"
#include "random.h"

constexpr auto BANA_API_VERSION = "Ver 1.6.1";

char hex_characters[] = {'0', '1' , '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E' ,'F'};

bool readerActive = false;

using Random = effolkronium::random_static;

void randomHex(char str[], int length) {
    //hexadecimal characters
    for (int i = 0; i < length; i++)
    {
        str[i] = hex_characters[Random::get(0, 128) % 16];
    }
    str[length] = 0;
}

void randomNumberString(char str[], int length) {
    for (int i = 0; i < length; i++)
    {
        str[i] = Random::get('0', '9');
    }
    str[length] = 0;
}

std::string getProfileString(LPCSTR name, LPCSTR key, LPCSTR def, LPCSTR filename) {
    char temp[1024];
    const DWORD result = GetPrivateProfileStringA(name, key, def, temp, sizeof(temp), filename);
    return std::string(temp, result);
}

static const std::filesystem::path& GetCardPath()
{
    static std::filesystem::path result = GetBasePath() / "card.ini";
    
    return result;
}

void createCard()
{
    std::string cardPath = GetCardPath().string();
    if (std::filesystem::exists(GetCardPath()))
    {
        info("Card.ini found at %s", cardPath.c_str());
    }
    else
    {
        char generatedAccessCode[21] = "00000000000000000000";
        randomNumberString(generatedAccessCode, 20);
        WritePrivateProfileStringA("card", "accessCode", generatedAccessCode, cardPath.c_str());

        char generatedChipId[33] = "00000000000000000000000000000000";
        randomHex(generatedChipId, 32);
        WritePrivateProfileStringA("card", "chipId", generatedChipId, cardPath.c_str());

        info("New card generated at %s", cardPath.c_str());
    }
}

void StartAttachThread(long (*callback)(long, long, long*), long* someStructPtr) {
    // this is a really ugly hack, forgive me
    using namespace std::chrono_literals;
    std::this_thread::sleep_for(100ms);

    callback(0, 0, someStructPtr);
}

void StartResetThread(long (*callback)(int, int, long*), long* someStructPtr) {
    // this is a really ugly hack, forgive me
    using namespace std::chrono_literals;
    std::this_thread::sleep_for(100ms);

    callback(0, 0, someStructPtr);
}

void StartReqActionThread(void (*callback)(long, int, long*), long* someStructPtr) {
    // this is a really ugly hack, forgive me
    using namespace std::chrono_literals;
    std::this_thread::sleep_for(100ms);

    callback(0, 0, someStructPtr);
}

void StartReadThread(void (*callback)(int, int, void*, void*), void* cardStuctPtr) {
    while (readerActive)
    {
        using namespace std::chrono_literals;
        std::this_thread::sleep_for(100ms);

        if (InputState::Get().Card)
        {
            // Raw card data and some other stuff, who cares
            unsigned char rawCardData[168] = {
                0x01, 0x01, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x92, 0x2E, 0x58, 0x32, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x7F, 0x5C, 0x97, 0x44, 0xF0, 0x88, 0x04, 0x00,
                0x43, 0x26, 0x2C, 0x33, 0x00, 0x04, 0x06, 0x10, 0x30, 0x30, 0x30, 0x30,
                0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30,
                0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30,
                0x30, 0x30, 0x30, 0x30, 0x00, 0x00, 0x00, 0x00, 0x30, 0x30, 0x30, 0x30,
                0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30,
                0x30, 0x30, 0x30, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
                0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x4E, 0x42, 0x47, 0x49, 0x43, 0x36, 0x00, 0x00, 0xFA, 0xE9, 0x69, 0x00,
                0xF6, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            INIReader reader(GetCardPath().string());
            std::string accessCode = "30764352518498791337";
            std::string chipId = "7F5C9744F111111143262C3300040610";
            if (reader.ParseError() == 0)
            {
                accessCode = reader.Get("card", "accessCode", "30764352518498791337");
                chipId = reader.Get("card", "chipId", "7F5C9744F111111143262C3300040610");
            }

            memcpy(rawCardData + 0x50, accessCode.c_str(), accessCode.size() + 1);
            memcpy(rawCardData + 0x2C, chipId.c_str(), chipId.size() + 1);

            trace("Callback from read card");
            callback(0, 0, rawCardData, cardStuctPtr);
        }
    }
}

static HINSTANCE bngrwOrig = LoadLibrary(TEXT("bngrw_orig")); 

typedef ULONGLONG (*BngRwAttach_Ori)(UINT a1, char* a2, int a3, int a4, long (*callback)(long, long, long*), long* some_struct_ptr);
typedef int (*BngRwDevReset_Ori)(UINT, long (*callback)(int, int, long*), long*);
typedef ULONGLONG (*BngRwExReadMifareAllBlock_Ori)();
typedef void (*BngRwFin_Ori)();
typedef UINT (*BngRwGetFwVersion_Ori)(UINT);
typedef UINT (*BngRwGetStationID_Ori)(UINT);
typedef UINT (*BngRwGetTotalRetryCount_Ori)(UINT);
typedef const char* (*BngRwGetVersion_Ori)();
typedef long (*BngRwInit_Ori)();
typedef ULONGLONG (*BngRwIsCmdExec_Ori)(UINT);
typedef int (*BngRwReqAction_Ori)(UINT, UINT, void (*callback)(long, int, long*), long* some_struct_ptr);
typedef int (*BngRwReqAiccAuth_Ori)(UINT, int, UINT, int*, ULONGLONG, ULONGLONG, ULONGLONG*);
typedef int (*BngRwReqBeep_Ori)(UINT, UINT, ULONGLONG, ULONGLONG);
typedef int (*BngRwReqCancel_Ori)(UINT);
typedef int (*BngRwReqFwCleanup_Ori)(UINT, ULONGLONG, ULONGLONG);
typedef int (*BngRwReqFwVersionup_Ori)(UINT, ULONGLONG, ULONGLONG, ULONGLONG);
typedef int (*BngRwReqLatchID_Ori)(UINT, ULONGLONG, ULONGLONG);
typedef int (*BngRwReqLed_Ori)(UINT, UINT, ULONGLONG, ULONGLONG);
typedef int (*BngRwReqSendMailTo_Ori)(UINT, int, UINT, int*, char*, char*, char*, char*, ULONGLONG, ULONGLONG);
typedef int (*BngRwReqSendUrlTo_Ori)(UINT, int, UINT, int*, char*, char*, ULONGLONG, ULONGLONG);
typedef int (*BngRwReqWaitTouch_Ori)(UINT a, int maxIntSomehow, UINT c, void (*callback)(int, int, void*, void*), void* card_struct_ptr);
typedef ULONGLONG (*BngRwReqSetLedPower_Ori)();

extern "C" {

ULONGLONG BngRwAttach(UINT a1, char* a2, int a3, int a4, long (*callback)(long, long, long*), long* some_struct_ptr) {
    trace("BngRwAttach(%i, %s, %d, %d, %p, %p)\n", a1, a2, a3, a4, callback, some_struct_ptr);

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        info("Real Card Reader Attach with COM Port %s Specified\n", globalConfig.CardReaderComPort.data());
        BngRwAttach_Ori originalFunction = reinterpret_cast<BngRwAttach_Ori>(GetProcAddress(bngrwOrig, "BngRwAttach"));
        return originalFunction(a1, globalConfig.CardReaderComPort.data(), a3, a4, callback, some_struct_ptr);
    }
    
    createCard();

    std::thread t(StartAttachThread, callback, some_struct_ptr);
    t.detach();
    return 1;
}

long BngRwInit() {
    trace("BngRwInit()\n");

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwInit_Ori originalFunction = reinterpret_cast<BngRwInit_Ori>(GetProcAddress(bngrwOrig, "BngRwInit"));
        return originalFunction();
    }
    
    return 0;
}

ULONGLONG BngRwReqSetLedPower() {
    trace("BngRwReqSetLedPower()\n");

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwReqSetLedPower_Ori originalFunction = reinterpret_cast<BngRwReqSetLedPower_Ori>(GetProcAddress(bngrwOrig, "BngRwReqSetLedPower"));
        return originalFunction();
    }
    
    return 0;
}

int BngRwDevReset(UINT a, long (*callback)(int, int, long*), long* some_struct_ptr) {
    trace("BngRwDevReset(%i, %p, %p)\n", a, callback, some_struct_ptr);

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwDevReset_Ori originalFunction = reinterpret_cast<BngRwDevReset_Ori>(GetProcAddress(bngrwOrig, "BngRwDevReset"));
        return originalFunction(a, callback, some_struct_ptr);
    }

    std::thread t(StartResetThread, callback, some_struct_ptr);
    t.detach();
    return 1;
}

ULONGLONG BngRwExReadMifareAllBlock() {
    trace("BngRwExReadMifareAllBlock()\n");

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwExReadMifareAllBlock_Ori originalFunction = reinterpret_cast<BngRwExReadMifareAllBlock_Ori>(GetProcAddress(bngrwOrig, "BngRwExReadMifareAllBlock"));
        return originalFunction();
    }
    
    return 0xffffff9c;
}

// Finalise?
void BngRwFin() {
    trace("BngRwFin()\n");

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwFin_Ori originalFunction = reinterpret_cast<BngRwFin_Ori>(GetProcAddress(bngrwOrig, "BngRwFin"));
        originalFunction();
    }
}

UINT BngRwGetFwVersion(UINT a) {
    trace("BngRwGetFwVersion(%i)\n", a);

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwGetFwVersion_Ori originalFunction = reinterpret_cast<BngRwGetFwVersion_Ori>(GetProcAddress(bngrwOrig, "BngRwGetFwVersion"));
        return originalFunction(a);
    }
    
    return 0;
}

UINT BngRwGetStationID(UINT a) {
    trace("BngRwGetStationID(%i)\n", a);

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwGetStationID_Ori originalFunction = reinterpret_cast<BngRwGetStationID_Ori>(GetProcAddress(bngrwOrig, "BngRwGetStationID"));
        return originalFunction(a);
    }
    
    return 0;
}

const char* BngRwGetVersion() {
    trace("BngRwGetVersion()\n");

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwGetVersion_Ori originalFunction = reinterpret_cast<BngRwGetVersion_Ori>(GetProcAddress(bngrwOrig, "BngRwGetVersion"));
        return originalFunction();
    }
    
    return BANA_API_VERSION;
}

ULONGLONG BngRwIsCmdExec(UINT a) {
    trace("BngRwIsCmdExec(%i)\n", a);

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwIsCmdExec_Ori originalFunction = reinterpret_cast<BngRwIsCmdExec_Ori>(GetProcAddress(bngrwOrig, "BngRwIsCmdExec"));
        return originalFunction(a);
    }
    
    // return 0xFFFFFFFF;
    return 0;
}

UINT BngRwGetTotalRetryCount(UINT a) {
    trace("BngRwGetTotalRetryCount(%i)\n", a);

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwGetTotalRetryCount_Ori originalFunction = reinterpret_cast<BngRwGetTotalRetryCount_Ori>(GetProcAddress(bngrwOrig, "BngRwGetTotalRetryCount"));
        return originalFunction(a);
    }
    
    return 0;
}

int BngRwReqLed(UINT a, UINT b, ULONGLONG c, ULONGLONG d) {
    trace("BngRwReqLed(%i, %i, %llu, %llu)\n", a, b, c, d);
    
    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwReqLed_Ori originalFunction = reinterpret_cast<BngRwReqLed_Ori>(GetProcAddress(bngrwOrig, "BngRwReqLed"));
        return originalFunction(a, b, c, d);
    }
    
    return 1;
}

int BngRwReqAction(UINT a, UINT b, void (*callback)(long, int, long*), long* some_struct_ptr) {
    trace("BngRwReqAction(%i, %i, %p, %p)\n", a, b, callback, some_struct_ptr);

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwReqAction_Ori originalFunction = reinterpret_cast<BngRwReqAction_Ori>(GetProcAddress(bngrwOrig, "BngRwReqAction"));
        return originalFunction(a, b, callback, some_struct_ptr);
    }

    std::thread t(StartReqActionThread, callback, some_struct_ptr);
    t.detach();
    if (b == 0)
    {
        return 1;
    }
    return -1;
}

int BngRwReqAiccAuth(UINT a, int b, UINT c, int* d, ULONGLONG e, ULONGLONG f, ULONGLONG* g) {
    trace("BngRwReqAiccAuth(%i, %d, %i, %p, %llu, %llu, %p)\n", a, b, c, d, e, f, g);

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwReqAiccAuth_Ori originalFunction = reinterpret_cast<BngRwReqAiccAuth_Ori>(GetProcAddress(bngrwOrig, "BngRwReqAiccAuth"));
        return originalFunction(a, b, c, d, e, f, g);
    }
    
    return 1;
}

int BngRwReqBeep(UINT a, UINT b, ULONGLONG c, ULONGLONG d) {
    trace("BngRwReqBeep(%i, %i, %llu, %llu)\n", a, b, c, d);

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwReqBeep_Ori originalFunction = reinterpret_cast<BngRwReqBeep_Ori>(GetProcAddress(bngrwOrig, "BngRwReqBeep"));
        return originalFunction(a, b, c, d);
    }
    
    return 1;
}

int BngRwReqCancel(UINT a) {
    trace("BngRwReqCancel(%i)\n", a);

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwReqCancel_Ori originalFunction = reinterpret_cast<BngRwReqCancel_Ori>(GetProcAddress(bngrwOrig, "BngRwReqCancel"));
        return originalFunction(a);
    }
    
    if (7 < a)
    {
        return -100;
    }
    return 1;
}

int BngRwReqFwCleanup(UINT a, ULONGLONG b, ULONGLONG c) {
    trace("BngRwReqFwCleanup(%i, %llu, %llu)\n", a, b, c);

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwReqFwCleanup_Ori originalFunction = reinterpret_cast<BngRwReqFwCleanup_Ori>(GetProcAddress(bngrwOrig, "BngRwReqFwCleanup"));
        return originalFunction(a, b, c);
    }
    
    return 1;
}

int BngRwReqFwVersionup(UINT a, ULONGLONG b, ULONGLONG c, ULONGLONG d) {
    trace("BngRwReqFwVersionup(%i, %llu, %llu, %llu)\n", a, b, c, d);

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwReqFwVersionup_Ori originalFunction = reinterpret_cast<BngRwReqFwVersionup_Ori>(GetProcAddress(bngrwOrig, "BngRwReqFwVersionup"));
        return originalFunction(a, b, c, d);
    }
    
    return 1;
}

int BngRwReqLatchID(UINT a, ULONGLONG b, ULONGLONG c) {
    trace("BngRwReqLatchId(%i, %llu, %llu)\n", a, b, c);

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwReqLatchID_Ori originalFunction = reinterpret_cast<BngRwReqLatchID_Ori>(GetProcAddress(bngrwOrig, "BngRwReqLatchID"));
        return originalFunction(a, b, c);
    }
    
    if (a < 8)
    {
        return -100;
    }
    return 1;
}

int BngRwReqSendMailTo(UINT a, int b, UINT c, int* d,
                       char* e, char* f, char* g, char* h, ULONGLONG i, ULONGLONG j) {
    trace("BngRwReqSendMailTo(%i, %d, %i, %p, %s, %s, %s, %s, %llu, %llu)\n", a, b, c, d, e, f, g, h, i, j);
    if (7 < a)
    {
        return -100;
    }
    if (!e)
    {
        return -100;
    }
    return 1;
}

int BngRwReqSendUrlTo(UINT a, int b, UINT c, int* d,
                      char* e, char* f, ULONGLONG g, ULONGLONG h) {
    trace("BngRwReqSendUrlTo(%i, %d, %i, %p, %s, %s, %llu, %llu)\n", a, b, c, d, e, f, g, h);
    if (7 < a)
    {
        return -100;
    }
    if (!e)
    {
        return -100;
    }
    return 1;
}

int BngRwReqWaitTouch(UINT a, int maxIntSomehow, UINT c, void (*callback)(int, int, void*, void*), void* card_struct_ptr) {
    trace("BngRwReqWaitTouch(%i, %d, %i, %p, %p)\n", a, maxIntSomehow, c, callback, card_struct_ptr);

    if(globalConfig.UseRealCardReader == true && bngrwOrig != nullptr)
    {
        BngRwReqWaitTouch_Ori originalFunction = (BngRwReqWaitTouch_Ori) GetProcAddress(bngrwOrig, "BngRwReqWaitTouch");
        return originalFunction(a, maxIntSomehow, c, callback, card_struct_ptr);
    }

    // Hack to make sure previous threads have exited
    readerActive = false;
    using namespace std::chrono_literals;
    std::this_thread::sleep_for(250ms);

    readerActive = true;

    std::thread t(StartReadThread, callback, card_struct_ptr);
    t.detach();

    return 1;
}
}
