#include "AmAuthEmu.h"
#include "Windows.h"
#include <format>

#include "INIReader.h"

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
static config_struct amconfig;

class CAuth : public IUnknown
{
public:
    STDMETHODIMP
    QueryInterface(REFIID riid, LPVOID* ppvObj)
    {
        wchar_t* iid_str;
        StringFromCLSID(riid, &iid_str);
        OutputDebugStringW(std::format(L"QueryInterface %ls\n", iid_str).c_str());

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
        OutputDebugStringA("Unk3");
        return 1;
    }

    virtual int64_t
    Unk4()
    {
        OutputDebugStringA("Unk4");
        return 1;
    }

    virtual int32_t
    Unk5()
    {
        OutputDebugStringA("Unk5");
        return 0;
    }

    virtual int64_t
    Unk6()
    {
        OutputDebugStringA("Unk6");
        return 1;
    }

    virtual int32_t
    Unk7()
    {
        OutputDebugStringA("Unk7");
        return 0;
    }

    virtual int32_t
    Unk8()
    {
        OutputDebugStringA("Unk8");
        return 0;
    }

    virtual int32_t
    Unk9(int32_t* a1)
    {
        OutputDebugStringA("Unk9");
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
        OutputDebugStringA("IAuth_GetCabinetConfig");
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
        /*strcpy_s(result.pcbid, amconfig.PcbId.c_str());
        strcpy_s(result.dongle_serial, amconfig.Serial.c_str());
        strcpy_s(result.shop_router_ip, amconfig.TenpoRouter.c_str());
        strcpy_s(result.auth_server_ip, amconfig.AuthServerIp.c_str());
        strcpy_s(result.local_ip, amconfig.IpAddress.c_str());
        strcpy_s(result.subnet_mask, amconfig.SubnetMask.c_str());
        strcpy_s(result.gateway, amconfig.Gateway.c_str());
        strcpy_s(result.primary_dns, amconfig.PrimaryDNS.c_str());*/
        memcpy_s(state, sizeof(amcus_network_state_t), &result, sizeof(amcus_network_state_t));
        return 0;
    }

    virtual int32_t
    IAuth_GetVersionInfo(amcus_version_info_t* version)
    {
        OutputDebugStringA("IAuth_GetVersionInfo");
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
        OutputDebugStringA("Unk12");
        return 1;
    }

    virtual int32_t
    Unk13()
    {
        OutputDebugStringA("Unk13");
        return 1;
    }

    virtual int32_t
    IAuth_GetAuthServerResp(amcus_auth_server_resp_t* resp)
    {
        OutputDebugStringA("IAuth_GetAuthServerResp");
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
        /*strcpy_s(result.uri, amconfig.ServerAddress.c_str());
        strcpy_s(result.host, amconfig.ServerAddress.c_str());*/
        memcpy_s(resp, sizeof(amcus_auth_server_resp_t), &result, sizeof(amcus_auth_server_resp_t));
        return 0;
    }

    virtual int32_t
    Unk15()
    {
        OutputDebugStringA("Unk15");
        return 0;
    }

    virtual int32_t
    Unk16()
    {
        OutputDebugStringA("Unk16");
        return 0;
    }

    virtual int32_t
    Unk17()
    {
        OutputDebugStringA("Unk17");
        return 0;
    }

    virtual int32_t
    Unk18(void* a1)
    {
        OutputDebugStringA("Unk18");
        return 0;
    }

    virtual int32_t
    Unk19(uint8_t* a1)
    {
        OutputDebugStringA("Unk19");
        memset(a1, 0, 0x38);
        a1[0] = 1;
        return 1;
    }

    virtual int32_t
    Unk20()
    {
        OutputDebugStringA("Unk20");
        return 0;
    }

    virtual int32_t
    Unk21()
    {
        OutputDebugStringA("Unk21");
        return 1;
    }

    virtual int32_t
    Unk22()
    {
        OutputDebugStringA("Unk22");
        return 0;
    }

    virtual int32_t
    Unk23()
    {
        OutputDebugStringA("Unk23");
        return 0;
    }

    virtual int32_t
    Unk24()
    {
        OutputDebugStringA("Unk24");
        return 0;
    }

    virtual int32_t
    Unk25()
    {
        OutputDebugStringA("Unk25");
        return 1;
    }

    virtual int32_t
    Unk26()
    {
        OutputDebugStringA("Unk26");
        return 0;
    }

    virtual int32_t
    Unk27()
    {
        OutputDebugStringA("Unk27");
        return 1;
    }

    virtual int32_t
    Unk28()
    {
        OutputDebugStringA("Unk28");
        return 0;
    }

    virtual int32_t
    Unk29()
    {
        OutputDebugStringA("Unk29");
        return 0;
    }

    virtual int32_t
    Unk30()
    {
        OutputDebugStringA("Unk30");
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
        OutputDebugStringW(std::format(L"QueryInterface %ls\n", iid_str).c_str());

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
        if (outer != 0) return CLASS_E_NOAGGREGATION;
        wchar_t* iid_str;
        StringFromCLSID(riid, &iid_str);
        printf("CreateInstance %ls\n", iid_str);
        CAuth* auth = new CAuth();
        return auth->QueryInterface(riid, object);
    }

    virtual HRESULT
    LockServer(int32_t lock)
    {
        return 0;
    }
};

void InitAmAuthEmu(config_struct &config)
{
    CoInitializeEx(0, 0);
    auto result = CoRegisterClassObject(IID_CAuthFactory, new CAuthFactory(), CLSCTX_LOCAL_SERVER, REGCLS_MULTIPLEUSE,
                                        &reg);
    if (result != S_OK)
    {
        OutputDebugStringA("Error");
    }
    memcpy_s(&amconfig, sizeof(config_struct), &config, sizeof(config_struct));
}

void ExitAmAuthEmu()
{
    CoRevokeClassObject(reg);
    CoUninitialize();
}
