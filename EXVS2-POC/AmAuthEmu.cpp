#include "AmAuthEmu.h"
#include "Windows.h"
#include <format>

//DEFINE_GUID (IID_CAuthFactory, 0x4603BB03, 0x058D, 0x43D9, 0xB9, 0x6F, 0x63, 0x9B, 0xE9, 0x08, 0xC1, 0xED);
//DEFINE_GUID (IID_CAuth, 0x045A5150, 0xD2B3, 0x4590, 0xA3, 0x8B, 0xC1, 0x15, 0x86, 0x78, 0xE1, 0xAC);

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

DWORD reg = 0;
class CAuth : public IUnknown {
  public:
	STDMETHODIMP
	QueryInterface (REFIID riid, LPVOID *ppvObj) {
		wchar_t *iid_str;
		StringFromCLSID (riid, &iid_str);
		OutputDebugStringW (std::format(L"QueryInterface %ls\n", iid_str).c_str());

		if (riid == IID_IUnknown || riid == IID_CAuth) {
			*ppvObj = this;
			this->AddRef ();
			return 0;
		} else {
			*ppvObj = 0;
			return E_NOINTERFACE;
		}
	}
	STDMETHODIMP_ (ULONG) AddRef () { return this->refCount++; }
	STDMETHODIMP_ (ULONG) Release () {
		this->refCount--;
		if (this->refCount <= 0) {
			// delete this;
			return 0;
		}
		return this->refCount;
	}

	virtual int64_t
	Unk3 (uint32_t a1) {
		OutputDebugStringA("Unk3");
		return 1;
	}
	virtual int64_t
	Unk4 () {
		OutputDebugStringA("Unk4");
		return 1;
	}
	virtual int32_t
	Unk5 () {
		OutputDebugStringA("Unk5");
		return 0;
	}
	virtual int64_t
	Unk6 () {
		OutputDebugStringA("Unk6");
		return 1;
	}
	virtual int32_t
	Unk7 () {
		OutputDebugStringA("Unk7");
		return 0;
	}
	virtual int32_t
	Unk8 () {
		OutputDebugStringA("Unk8");
		return 0;
	}
	virtual int32_t
	Unk9 (int32_t *a1) {
		OutputDebugStringA("Unk9");
		memset (a1, 0, sizeof (int32_t) * 0x31);
		a1[0]  = 15;
		a1[2]  = 2;
		a1[3]  = 1;
		a1[6]  = 9;
		a1[8]  = 2;
		a1[9]  = 1;
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
	Unk10 (char *a1) {
		OutputDebugStringA("Unk10");
		memset (a1, 0, 0xB0);
		strncpy_s (a1, 0x10, "CLIENT", 0xF);
		strncpy_s (a1 + 0x10, 0x10, "ABLN1110001", 0xF);         // PCB ID
		strncpy_s (a1 + 0x20, 0x10, "284311110001", 0xF); // Serial
		strncpy_s (a1 + 0x30, 0x10, "192.168.50.239", 0xF);
		strncpy_s (a1 + 0x40, 0x10, "192.168.50.239", 0xF);
		strncpy_s (a1 + 0x50, 0x10, "192.168.50.239", 0xF);
		strncpy_s (a1 + 0x60, 0x10, "255.255.255.0", 0xF); // Subnet mask
		strncpy_s (a1 + 0x70, 0x10, "192.168.50.1", 0xF); // GATEWAY
		strncpy_s (a1 + 0x80, 0x10, "***.***.***.***", 0xF); // PRIMARY DNS
		return 0;
	}
	virtual int32_t
	Unk11 (char *a1) {
		OutputDebugStringA("Unk11");
		memset (a1, 0, 0x13C);
		strncpy_s (a1, 4, "1", 3);
		strncpy_s (a1 + 4, 0x10, "ALLNET", 0xF);
		strncpy_s (a1 + 20, 8, "SBUZ", 7);
		strncpy_s (a1 + 28, 8, "4.50", 7);
		strncpy_s (a1 + 36, 8, "TAL0", 7);  
		strncpy_s (a1 + 44, 8, "27.35", 7); // GAME VERSION
		strncpy_s (a1 + 52, 4, "0", 3);
		strncpy_s (a1 + 56, 4, "PCB", 3);
		return 0;
	}
	virtual int32_t
	Unk12 () {
		OutputDebugStringA("Unk12");
		return 1;
	}
	virtual int32_t
	Unk13 () {
		OutputDebugStringA("Unk13");
		return 1;
	}
	virtual int32_t
	Unk14 (char *a1) {
		OutputDebugStringA("Unk14");
		memset (a1, 0, 0x8A2);
		strncpy_s (a1, 0x101, "vsapi.taiko-p.jp", 0x100);
		strncpy_s (a1 + 0x101, 0x101, "vsapi.taiko-p.jp", 0x100);
		strncpy_s (a1 + 0x202, 0x100, "TAIKO ARCADE LOADER", 0xFF); // ALL.Net SHOP NAME
		strncpy_s (a1 + 0x302, 0x100, "TAIKO ARCADE LOADER", 0xFF);
		strncpy_s (a1 + 0x402, 0x10, "1", 0xF);
		strncpy_s (a1 + 0x412, 0x100, "TAIKO ARCADE LOADER", 0xFF);
		strncpy_s (a1 + 0x512, 0x100, "X", 0xFF);
		strncpy_s (a1 + 0x612, 0x100, "Y", 0xFF);
		strncpy_s (a1 + 0x712, 0x100, "Z", 0xFF);
		strncpy_s (a1 + 0x812, 0x10, "JPN0123", 0xF);
		strncpy_s (a1 + 0x832, 0x10, "JPN", 0xF);
		strncpy_s (a1 + 0x842, 0x10, "002,00", 0xF);
		strncpy_s (a1 + 0x842, 0x10, "PowerOnResponseVer2", 0xF);
		return 0;
	}
	virtual int32_t
	Unk15 () {
		OutputDebugStringA("Unk15");
		return 0;
	}
	virtual int32_t
	Unk16 () {
		OutputDebugStringA("Unk16");
		return 0;
	}
	virtual int32_t
	Unk17 () {
		OutputDebugStringA("Unk17");
		return 0;
	}
	virtual int32_t
	Unk18 (void *a1) {
		OutputDebugStringA("Unk18");
		return 0;
	}
	virtual int32_t
	Unk19 (uint8_t *a1) {
		OutputDebugStringA("Unk19");
		memset (a1, 0, 0x38);
		// a1[0] = 1;
		return 1;
	}
	virtual int32_t
	Unk20 () {
		OutputDebugStringA("Unk20");
		return 0;
	}
	virtual int32_t
	Unk21 () {
		OutputDebugStringA("Unk21");
		return 1;
	}
	virtual int32_t
	Unk22 () {
		OutputDebugStringA("Unk22");
		return 0;
	}
	virtual int32_t
	Unk23 () {
		OutputDebugStringA("Unk23");
		return 0;
	}
	virtual int32_t
	Unk24 () {
		OutputDebugStringA("Unk24");
		return 0;
	}
	virtual int32_t
	Unk25 () {
		OutputDebugStringA("Unk25");
		return 1;
	}
	virtual int32_t
	Unk26 () {
		OutputDebugStringA("Unk26");
		return 0;
	}
	virtual int32_t
	Unk27 () {
		OutputDebugStringA("Unk27");
		return 1;
	}
	virtual int32_t
	Unk28 () {
		OutputDebugStringA("Unk28");
		return 0;
	}
	virtual int32_t
	Unk29 () {
		OutputDebugStringA("Unk29");
		return 0;
	}
	virtual int32_t
	Unk30 () {
		OutputDebugStringA("Unk30");
		return 0;
	}
	virtual int32_t
	PrintDebugInfo () {
		return 0;
	}
	virtual int32_t
	Unk32 (void *a1) {
		return 0;
	}
	virtual void
	Unk33 () {}

  public:
	CAuth () {}
	virtual ~CAuth () {}

  private:
	int32_t refCount = 0;
};


class CAuthFactory : public IClassFactory {
public:
    STDMETHODIMP
    QueryInterface (REFIID riid, LPVOID *ppvObj) {
        wchar_t *iid_str;
        StringFromCLSID (riid, &iid_str);
        OutputDebugStringW (std::format(L"QueryInterface %ls\n", iid_str).c_str());

        if (riid == IID_IUnknown || riid == IID_IClassFactory || riid == IID_CAuthFactory) {
            *ppvObj = this;
            return 0;
        } else {
            *ppvObj = 0;
            return E_NOINTERFACE;
        }
    }
    STDMETHODIMP_ (ULONG) AddRef () { return 2; }
    STDMETHODIMP_ (ULONG) Release () { return 1; }
    virtual HRESULT
    CreateInstance (IUnknown *outer, REFIID riid, void **object) {
        if (outer != 0) return CLASS_E_NOAGGREGATION;
        wchar_t *iid_str;
        StringFromCLSID (riid, &iid_str);
        printf ("CreateInstance %ls\n", iid_str);
        CAuth *auth = new CAuth ();
        return auth->QueryInterface (riid, object);
    }
    virtual HRESULT
    LockServer (int32_t lock) {
        return 0;
    }
};

void InitAmAuthEmu()
{
	CoInitializeEx (0, 0);
	auto result = CoRegisterClassObject (IID_CAuthFactory, new CAuthFactory(), CLSCTX_LOCAL_SERVER, REGCLS_MULTIPLEUSE, &reg);
	if (result != S_OK)
	{
		OutputDebugStringA("Error");
	}
}

void ExitAmAuthEmu()
{
	CoRevokeClassObject (reg);
	CoUninitialize ();
}

