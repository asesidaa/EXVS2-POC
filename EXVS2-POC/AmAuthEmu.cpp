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

DWORD reg = 0;
static config_struct amconfig {};

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
        log("Unk3");
        return 1;
    }

    virtual int64_t
    Unk4()
    {
        log("Unk4");
        return 1;
    }

    virtual int32_t
    Unk5()
    {
        log("Unk5");
        return 0;
    }

    virtual int64_t
    Unk6()
    {
        log("Unk6");
        return 1;
    }

    virtual int32_t
    Unk7()
    {
        log("Unk7");
        return 0;
    }

    virtual int32_t
    Unk8()
    {
        log("Unk8");
        return 0;
    }

    virtual int32_t
    Unk9(int32_t* a1)
    {
        log("Unk9");
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
        log("IAuth_GetCabinetConfig");
        amcus_network_state_t result
        {
            .mode = "CLIENT",
            .pcbid = "ABLN1110001",
            .dongle_serial = "284311110001",
            .shop_router_ip = "192.168.50.239",
            .auth_server_ip = "127.0.0.1",
            .local_ip = "192.168.50.239",
            .subnet_mask = "255.255.255.0",
            .gateway = "192.168.50.1",
            .primary_dns = "8.8.8.8",
            .hop_count = 1,
            .line_type = 1,
            .line_status = 1,
            .content_router_status = 1,
            .shop_router_status = 1,
            .hop_status = 1
        };
        strcpy_s(result.pcbid, amconfig.PcbId.c_str());
        strcpy_s(result.dongle_serial, amconfig.Serial.c_str());
        strcpy_s(result.shop_router_ip, amconfig.TenpoRouter.c_str());
        strcpy_s(result.auth_server_ip, amconfig.AuthServerIp.c_str());
        strcpy_s(result.local_ip, amconfig.IpAddress.c_str());
        strcpy_s(result.subnet_mask, amconfig.SubnetMask.c_str());
        strcpy_s(result.gateway, amconfig.Gateway.c_str());
        strcpy_s(result.primary_dns, amconfig.PrimaryDNS.c_str());
        memcpy_s(state, sizeof(amcus_network_state_t), &result, sizeof(amcus_network_state_t));
        return 0;
    }

    virtual int32_t
    IAuth_GetVersionInfo(amcus_version_info_t* version)
    {
        log("IAuth_GetVersionInfo");
        amcus_version_info_t version_info
        {
            .game_rev = "1",
            .auth_type = "ALL.NET",
            .game_id = "SBUZ",
            .game_ver = "4.50",
            .game_cd = "GXX1",
            .cacfg_game_ver = "27.35",
            .game_board_type = "0",
            .game_board_id = "PCB",
            .auth_url = "localhost"
        };
        memcpy_s(version, sizeof(amcus_version_info_t), &version_info, sizeof(amcus_version_info_t));
        return 0;
    }

    virtual int32_t
    Unk12()
    {
        log("Unk12");
        return 1;
    }

    virtual int32_t
    Unk13()
    {
        log("Unk13");
        return 1;
    }

    virtual int32_t
    IAuth_GetAuthServerResp(amcus_auth_server_resp_t* resp)
    {
        log("IAuth_GetAuthServerResp");
        log("Server address %s", amconfig.ServerAddress.c_str());

        std::string region_name_0 = amconfig.RegionCode;

        if(amconfig.Mode == "3" || amconfig.Mode == "4")
        {
            region_name_0 = "01035";
        }
        
        amcus_auth_server_resp_t result
        {
            .uri = "127.0.0.1",
            .host = "127.0.0.1",
            .shop_name = "EXVS2-POC",
            .shop_nickname = "EXVS2-POC",
            .region0 = "1",
            .region_name0 = "NAMCO",
            .region_name1 = "X",
            .region_name2 = "Y",
            .region_name3 = "Z",
            .place_id = "JPN1",
            .setting = "",
            .country = "JPN",
            .timezone = "+0900",
            .res_class = "PowerOnResponseVer2"
        };
        strcpy_s(result.uri, amconfig.ServerAddress.c_str());
        strcpy_s(result.host, amconfig.ServerAddress.c_str());
        strcpy_s(result.region0, region_name_0.c_str());
        memcpy_s(resp, sizeof(amcus_auth_server_resp_t), &result, sizeof(amcus_auth_server_resp_t));
        return 0;
    }

    virtual int32_t
    Unk15()
    {
        log("Unk15");
        return 0;
    }

    virtual int32_t
    Unk16()
    {
        log("Unk16");
        return 0;
    }

    virtual int32_t
    Unk17()
    {
        log("Unk17");
        return 0;
    }

    virtual int32_t
    Unk18(void* a1)
    {
        log("Unk18");
        return 0;
    }

    virtual int32_t
    Unk19(uint8_t* a1)
    {
        log("Unk19");
        memset(a1, 0, 0x38);
        a1[0] = 1;
        return 1;
    }

    virtual int32_t
    Unk20()
    {
        log("Unk20");
        return 0;
    }

    virtual int32_t
    Unk21()
    {
        log("Unk21");
        return 1;
    }

    virtual int32_t
    Unk22()
    {
        log("Unk22");
        return 0;
    }

    virtual int32_t
    Unk23()
    {
        log("Unk23");
        return 0;
    }

    virtual int32_t
    Unk24()
    {
        log("Unk24");
        return 0;
    }

    virtual int32_t
    Unk25()
    {
        log("Unk25");
        return 1;
    }

    virtual int32_t
    Unk26()
    {
        log("Unk26");
        return 0;
    }

    virtual int32_t
    Unk27()
    {
        log("Unk27");
        return 1;
    }

    virtual int32_t
    Unk28()
    {
        log("Unk28");
        return 0;
    }

    virtual int32_t
    Unk29()
    {
        log("Unk29");
        return 0;
    }

    virtual int32_t
    Unk30()
    {
        log("Unk30");
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


class CAuthFactory : public IClassFactory
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
        wchar_t* iid_str;
        StringFromCLSID(riid, &iid_str);
#ifdef _DEBUG
        OutputDebugStringW(std::format(L"CreateInstance {}", iid_str).c_str());
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


static HRESULT (STDAPICALLTYPE *gOriCoCreateInstance)(
    const IID *const rclsid,
    LPUNKNOWN pUnkOuter,
    DWORD dwClsContext,
    const IID *const riid,
    LPVOID *ppv);


static HRESULT STDAPICALLTYPE CoCreateInstanceHook(
    const IID *const rclsid,
    LPUNKNOWN pUnkOuter,
    DWORD dwClsContext,
    const IID *const riid,
    LPVOID *ppv)
{
    if (IsEqualGUID(*rclsid, IID_CAuthFactory))
    {
        log("GUID CAuthFactory match");
        if (IsEqualGUID(*riid, IID_CAuth))
        {
            log("GUID CAuth Match");
            auto cauth = new CAuth();
            return cauth->QueryInterface(*riid, ppv);
        }
    }
    return gOriCoCreateInstance(rclsid, pUnkOuter, dwClsContext, riid, ppv);
}

void InitAmAuthEmu(const config_struct& config)
{
    MH_Initialize();
    MH_CreateHookApi(L"ole32.dll", "CoCreateInstance", CoCreateInstanceHook, reinterpret_cast<void**>(&gOriCoCreateInstance));
    MH_EnableHook(nullptr);
    
    //memcpy_s(&amconfig, sizeof(config_struct), &config, sizeof(config_struct));
    amconfig = config;
    log("AMconfig Server address: %s", amconfig.ServerAddress.c_str());
}
