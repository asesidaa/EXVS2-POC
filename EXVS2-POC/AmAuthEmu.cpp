#include "AmAuthEmu.h"
#include "Windows.h"
#include <format>

#include "MinHook.h"
#include "INIReader.h"
#include "log.h"

/*
 * Reference: https://gitea.tendokyu.moe/Hay1tsme/bananatools/src/branch/master/amcus/iauth.c
 * https://github.com/BroGamer4256/TaikoArcadeLoader/blob/master/plugins/amauth/dllmain.cpp
 */

const GUID IID_CAuth
{
    0x045A5150,
    0xD2B3,
    0x4590,
    {0xA3, 0x8B, 0xC1, 0x15, 0x86, 0x78, 0xE1, 0xAC}
};

const GUID IID_CAuthFactory
{
    0x4603BB03,
    0x058D,
    0x43D9,
    {0xB9, 0x6F, 0x63, 0x9B, 0xE9, 0x08, 0xC1, 0xED}
};

typedef struct amcus_network_state
{
    char mode[16];
    char pcbid[16];
    char dongle_serial[16];
    char shop_router_ip[16];
    char auth_server_ip[16];
    char local_ip[16];
    char subnet_mask[16];
    char gateway[16];
    char primary_dns[16];
    int hop_count;
    uint32_t line_type;
    uint32_t line_status;
    uint32_t content_router_status;
    uint32_t shop_router_status;
    uint32_t hop_status;
} amcus_network_state_t;

typedef struct amcus_auth_server_resp
{
    char uri[257];
    char host[257];
    char shop_name[256];
    char shop_nickname[256];
    char region0[16];
    char region_name0[256];
    char region_name1[256];
    char region_name2[256];
    char region_name3[256];
    char place_id[16];
    char setting[16];
    char country[16];
    char timezone[32];
    char res_class[64];
} amcus_auth_server_resp_t;

typedef struct amcus_version_info
{
    char game_rev[4];
    char auth_type[16];
    char game_id[8];
    char game_ver[8];
    char game_cd[8];
    char cacfg_game_ver[8];
    char game_board_type[4];
    char game_board_id[4];
    char auth_url[256];
} amcus_version_info_t;

class CAuth : public IUnknown
{
public:
    STDMETHODIMP
    QueryInterface(REFIID riid, LPVOID* ppvObj)
    {
        wchar_t* iid_str;
        StringFromCLSID(riid, &iid_str);
#ifdef _DEBUG
        OutputDebugStringW(std::format(L"QueryInterface {}\n", iid_str).c_str());
#endif

        if (riid == IID_IUnknown || riid == IID_CAuth)
        {
            *ppvObj = this;
            this->AddRef();
            return 0;
        }
        else
        {
            *ppvObj = 0;
            return E_NOINTERFACE;
        }
    }

    STDMETHODIMP_(ULONG) AddRef() { return this->refCount++; }
    STDMETHODIMP_(ULONG) Release()
    {
        this->refCount--;
        if (this->refCount <= 0)
        {
            // delete this;
            return 0;
        }
        return this->refCount;
    }

    virtual int64_t
    Unk3(uint32_t a1)
    {
        trace("Unk3");
        return 1;
    }

    virtual int64_t
    Unk4()
    {
        trace("Unk4");
        return 1;
    }

    virtual int32_t
    Unk5()
    {
        trace("Unk5");
        return 0;
    }

    virtual int64_t
    Unk6()
    {
        trace("Unk6");
        return 1;
    }

    virtual int32_t
    Unk7()
    {
        trace("Unk7");
        return 0;
    }

    virtual int32_t
    Unk8()
    {
        trace("Unk8");
        return 0;
    }

    virtual int32_t
    Unk9(int32_t* a1)
    {
        trace("Unk9");
        memset(a1, 0, sizeof(int32_t) * 0x31);
        a1[0] = 15;
        a1[2] = 2;
        a1[3] = 1;
        a1[6] = 9;
        a1[8] = 2;
        a1[9] = 1;
        a1[10] = 27;
        a1[11] = 33;
        a1[12] = 41;
        a1[13] = 50;
        a1[14] = 59;
        a1[15] = 1179656;
        a1[30] = 1;
        a1[46] = 1;
        a1[47] = 3;
        a1[48] = 9;
        return 0;
    }

    virtual int32_t
    IAuth_GetCabinetConfig(amcus_network_state_t* state)
    {
        trace("IAuth_GetCabinetConfig");
        memset(state, 0, sizeof(*state));
        strcpy_s(state->mode, "CLIENT");
        strcpy_s(state->pcbid, globalConfig.PcbId.c_str());
        strcpy_s(state->dongle_serial, globalConfig.Serial.c_str());
        strcpy_s(state->auth_server_ip, globalConfig.AuthServerIp.c_str());

        // These values are optional, but they'll have been filled in by SocketHook initialization (if not overridden via config).
        strcpy_s(state->local_ip, globalConfig.IpAddress->c_str());
        strcpy_s(state->subnet_mask, globalConfig.SubnetMask->c_str());
        strcpy_s(state->gateway, globalConfig.Gateway->c_str());
        strcpy_s(state->shop_router_ip, globalConfig.TenpoRouter->c_str());
        strcpy_s(state->primary_dns, globalConfig.PrimaryDNS->c_str());

        state->hop_count = 1;
        state->line_type = 1;
        state->line_status = 1;
        state->content_router_status = 1;
        state->shop_router_status = 1;
        state->hop_status = 1;
        return 0;
    }

    virtual int32_t
    IAuth_GetVersionInfo(amcus_version_info_t* version)
    {
        trace("IAuth_GetVersionInfo");
        memset(version, 0, sizeof(*version));
        strcpy_s(version->game_rev, "1");
        strcpy_s(version->auth_type, "ALL.NET");
        strcpy_s(version->game_id, "SBUZ");
        strcpy_s(version->game_ver, "4.50");
        strcpy_s(version->game_cd, "GXX1");
        strcpy_s(version->cacfg_game_ver, "27.35");
        strcpy_s(version->game_board_type, "0");
        strcpy_s(version->game_board_id, "PCB");
        strcpy_s(version->auth_url, "localhost");
        return 0;
    }

    virtual int32_t
    Unk12()
    {
        trace("Unk12");
        return 1;
    }

    virtual int32_t
    Unk13()
    {
        trace("Unk13");
        return 1;
    }

    virtual int32_t
    IAuth_GetAuthServerResp(amcus_auth_server_resp_t* resp)
    {
        trace("IAuth_GetAuthServerResp");
        debug("Server address %s", globalConfig.ServerAddress.c_str());

        memset(resp, 0, sizeof(*resp));
        strcpy_s(resp->uri, globalConfig.ServerAddress.c_str());
        strcpy_s(resp->host, globalConfig.ServerAddress.c_str());

        strcpy_s(resp->shop_name, "EXVS2-POC");
        strcpy_s(resp->shop_nickname, "EXVS2-POC");

        if (globalConfig.Mode == 3 || globalConfig.Mode == 4)
        {
            strcpy_s(resp->region0, "01035");
        }
        else
        {
            strcpy_s(resp->region0, globalConfig.RegionCode.c_str());
        }

        strcpy_s(resp->region_name0, "NAMCO");
        strcpy_s(resp->region_name1, "X");
        strcpy_s(resp->region_name2, "Y");
        strcpy_s(resp->region_name3, "Z");
        strcpy_s(resp->place_id, "JPN1");
        strcpy_s(resp->setting, "");
        strcpy_s(resp->country, "JPN");
        strcpy_s(resp->timezone, "+0900");
        strcpy_s(resp->res_class, "PowerOnResponseVer2");
        return 0;
    }

    virtual int32_t
    Unk15()
    {
        trace("Unk15");
        return 0;
    }

    virtual int32_t
    Unk16()
    {
        trace("Unk16");
        return 0;
    }

    virtual int32_t
    Unk17()
    {
        trace("Unk17");
        return 0;
    }

    virtual int32_t
    Unk18(void* a1)
    {
        trace("Unk18");
        return 0;
    }

    virtual int32_t
    Unk19(uint8_t* a1)
    {
        trace("Unk19");
        memset(a1, 0, 0x38);
        a1[0] = 1;
        return 1;
    }

    virtual int32_t
    Unk20()
    {
        trace("Unk20");
        return 0;
    }

    virtual int32_t
    Unk21()
    {
        trace("Unk21");
        return 1;
    }

    virtual int32_t
    Unk22()
    {
        trace("Unk22");
        return 0;
    }

    virtual int32_t
    Unk23()
    {
        trace("Unk23");
        return 0;
    }

    virtual int32_t
    Unk24()
    {
        trace("Unk24");
        return 0;
    }

    virtual int32_t
    Unk25()
    {
        trace("Unk25");
        return 1;
    }

    virtual int32_t
    Unk26()
    {
        trace("Unk26");
        return 0;
    }

    virtual int32_t
    Unk27()
    {
        trace("Unk27");
        return 1;
    }

    virtual int32_t
    Unk28()
    {
        trace("Unk28");
        return 0;
    }

    virtual int32_t
    Unk29()
    {
        trace("Unk29");
        return 0;
    }

    virtual int32_t
    Unk30()
    {
        trace("Unk30");
        return 0;
    }

    virtual int32_t
    PrintDebugInfo()
    {
        return 0;
    }

    virtual int32_t
    Unk32(void* a1)
    {
        return 0;
    }

    virtual void
    Unk33()
    {
    }

public:
    CAuth()
    {
    }

    virtual ~CAuth()
    {
    }

private:
    int32_t refCount = 0;
};


class CAuthFactory final : public IClassFactory
{
public:
    virtual ~CAuthFactory() = default;
    STDMETHODIMP
    QueryInterface(REFIID riid, LPVOID* ppvObj)
    {
        wchar_t* iid_str;
        StringFromCLSID(riid, &iid_str);
#ifdef _DEBUG
        OutputDebugStringW(std::format(L"QueryInterface {}\n", iid_str).c_str());
#endif

        if (riid == IID_IUnknown || riid == IID_IClassFactory || riid == IID_CAuthFactory)
        {
            *ppvObj = this;
            return 0;
        }
        else
        {
            *ppvObj = 0;
            return E_NOINTERFACE;
        }
    }

    STDMETHODIMP_(ULONG) AddRef() { return 2; }
    STDMETHODIMP_(ULONG) Release() { return 1; }

    virtual HRESULT
    CreateInstance(IUnknown* outer, REFIID riid, void** object)
    {
        if (outer != nullptr) return CLASS_E_NOAGGREGATION;
#ifdef _DEBUG
        wchar_t* iid_str;
        StringFromCLSID(riid, &iid_str);
        OutputDebugStringW(std::format(L"CreateInstance {}", iid_str).c_str());
        CoTaskMemFree(iid_str);
#endif

        CAuth* auth = new CAuth();
        return auth->QueryInterface(riid, object);
    }

    virtual HRESULT
    LockServer(int32_t lock)
    {
        return 0;
    }
};


static HRESULT (STDAPICALLTYPE* g_origCoCreateInstance)(
    const IID *const rclsid,
    LPUNKNOWN pUnkOuter,
    DWORD dwClsContext,
    const IID *const riid,
    LPVOID *ppv);


static HRESULT STDAPICALLTYPE CoCreateInstanceHook(
    const IID* const rclsid,
    LPUNKNOWN pUnkOuter,
    DWORD dwClsContext,
    const IID* const riid,
    LPVOID *ppv)
{
    HRESULT result;

    LPOLESTR clsidStr = nullptr;
    LPOLESTR iidStr = nullptr;
    StringFromIID(*rclsid, &clsidStr);
    StringFromIID(*riid, &iidStr);

    if (IsEqualGUID(*rclsid, IID_CAuthFactory) && IsEqualGUID(*riid, IID_CAuth))
    {
        auto cauth = new CAuth();
        result = cauth->QueryInterface(*riid, ppv);
        trace("CoCreateInstance (hooked) (clsid=%S, pUnk=%p, clsCtx=%d, riid=%S) = %d\n", clsidStr, pUnkOuter, dwClsContext, iidStr, result);
    }
    else
    {
        result = g_origCoCreateInstance(rclsid, pUnkOuter, dwClsContext, riid, ppv);
        trace("CoCreateInstance(clsid=%S, pUnk=%p, clsCtx=%d, riid=%S) = %d\n", clsidStr, pUnkOuter, dwClsContext, iidStr, result);
    }

    CoTaskMemFree(clsidStr);
    CoTaskMemFree(iidStr);
    return result;
}

void InitAmAuthEmu()
{
    MH_CreateHookApi(L"ole32.dll", "CoCreateInstance", CoCreateInstanceHook, reinterpret_cast<void**>(&g_origCoCreateInstance));  // NOLINT(clang-diagnostic-microsoft-cast)
}
