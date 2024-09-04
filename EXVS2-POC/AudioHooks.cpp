#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING
#include <codecvt>

#include "AudioHooks.h"

#include <Windows.h>

#include <Functiondiscoverykeys_devpkey.h>

#include <atomic>
#include <mutex>
#include <string>
#include <unordered_map>

#include "COMHooks.h"
#include "Configs.h"
#include "log.h"

struct WrappedDevice;

static const CLSID CLSID_MMDeviceEnumerator = __uuidof(MMDeviceEnumerator);
static const IID IID_IMMDeviceEnumerator = __uuidof(IMMDeviceEnumerator);

DEFINE_GUID(IID_IMMDevice, 0xd666063f, 0x1587, 0x4e43, 0x81, 0xf1, 0xb9, 0x48, 0xe8, 0x07, 0x36, 0x3f);

static WrappedDevice* selectedAudioDevice;
static std::wstring selectedAudioDeviceId;
static bool foundOverrideAudioDevice = false;

static bool convertLPWToString(std::string& s, const LPWSTR pw, UINT codepage = CP_ACP)
{
    bool res = false;
    char* p = 0;
    int bsz;

    bsz = WideCharToMultiByte(codepage, 0, pw,-1, 0,0,0,0);
    if (bsz > 0) {
        p = new char[bsz];
        int rc = WideCharToMultiByte(codepage,0,pw,-1,p,bsz,0,0);
        if (rc != 0) {
            p[bsz-1] = 0;
            s = p;
            res = true;
        }
    }
    delete [] p;
    return res;
}

static void DumpDevice(unsigned int index, LPWSTR id, IMMDevice* device, bool selected)
{
    IPropertyStore* propstore;

    if (device->OpenPropertyStore(STGM_READ, &propstore) != S_OK)
    {
        err("failed to open property store for audio device %u", index);
        return;
    }

    PROPVARIANT property;
    PropVariantInit(&property);

    if (propstore->GetValue(PKEY_Device_FriendlyName, &property) != S_OK)
    {
        err("failed to get friendly name for audio device %u", index);
        propstore->Release();
        return;
    }

    if (property.vt == VT_EMPTY)
    {
        warn("%s Audio device #%u: <no name> (%S)%s", selected ? "[>>>]" : "[   ]", index, id);
    }
    else
    {
        std::string nameString = "";
        std::string idString = "";
        convertLPWToString(nameString, property.pwszVal);
        convertLPWToString(idString, id);

        info("%s Audio device #%u: %s (%s)", selected ? "[>>>]" : "[   ]", index, nameString.c_str(), idString.c_str());
    }

    PropVariantClear(&property);
    propstore->Release();
}

// This runs upon the first call to IMMDeviceEnumerator instead of at startup, because we're not
// CoInitialize'd in dllmain.
static IMMDevice* FindAudioDevice(std::optional<std::wstring> expectedDeviceId)
{
    IMMDevice* result = nullptr;
    IMMDeviceEnumerator* enumerator;
    HRESULT rc;

    rc = OrigCoCreateInstance(CLSID_MMDeviceEnumerator, nullptr, CLSCTX_ALL, IID_IMMDeviceEnumerator,
                              reinterpret_cast<void**>(&enumerator));
    if (rc != S_OK)
    {
        err("failed to create IMMDeviceEnumerator: %#x", rc);
        return nullptr;
    }

    IMMDeviceCollection* devices;
    rc = enumerator->EnumAudioEndpoints(eRender, DEVICE_STATE_ACTIVE, &devices);
    if (rc != S_OK)
    {
        err("failed to enumerate audio endpoints: %#x", rc);
        return nullptr;
    }

    unsigned int count;
    rc = devices->GetCount(&count);
    if (rc != S_OK)
    {
        fatal("failed to get audio device count: %#x", rc);
    }

    for (unsigned int i = 0; i < count; ++i)
    {
        IMMDevice* device;
        rc = devices->Item(i, &device);
        if (rc != S_OK)
        {
            err("failed to get audio device %u: %#x", i, rc);
            continue;
        }

        LPWSTR deviceId;
        rc = device->GetId(&deviceId);
        if (rc != S_OK)
        {
            err("failed to get audio device %u id: %#x", i, rc);
        }

        bool selected = expectedDeviceId == deviceId;
        DumpDevice(i, deviceId, device, selected);

        if (selected)
        {
            result = device;
        }
        else
        {
            device->Release();
        }
    }

    devices->Release();
    enumerator->Release();

    return result;
}

void WrappedDeviceRegistry::InitializeOnce()
{
    std::call_once(once_, [this]() {
        std::optional<std::wstring> deviceId;
        if (globalConfig.Audio.Device)
        {
            deviceId = std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>, wchar_t>{}.from_bytes(
                *globalConfig.Audio.Device);
            info("Looking for audio device %S", deviceId->c_str());
        }
        else
        {
            info("No audio device specified, using default");
        }

        if (IMMDevice* device = FindAudioDevice(deviceId))
        {
            selectedAudioDevice = FromOriginal(device);
            selectedAudioDeviceId = *deviceId;
            foundOverrideAudioDevice = true;
        }
        else
        {
            info("No Audio Device specified");
            foundOverrideAudioDevice = false;
        }
    });
}

WrappedDevice* WrappedDeviceRegistry::FromOriginal(IMMDevice* originalDevice)
{
    std::lock_guard<std::mutex> lock(mutex_);
    auto it = wrapMapping.find(originalDevice);
    if (it != wrapMapping.end())
    {
        it->second->AddRef();
        return it->second;
    }
    auto result = new WrappedDevice(this, originalDevice);
    wrapMapping.insert({originalDevice, result});
    return result;
}

WrappedDeviceRegistry& WrappedDeviceRegistry::Instance()
{
    static WrappedDeviceRegistry instance;
    return instance;
}

void WrappedDeviceRegistry::Remove(IMMDevice* originalDevice)
{
    std::lock_guard<std::mutex> lock(mutex_);
    wrapMapping.erase(originalDevice);
}

WrappedDevice::WrappedDevice(WrappedDeviceRegistry* registry, IMMDevice* original)
    : refcount_(1), registry_(registry), original_(original)
{
    debug("WrappedDevice constructed from original %p", original);
}

WrappedDevice::~WrappedDevice()
{
    registry_->Remove(original_);
}

STDMETHODIMP WrappedDevice::QueryInterface(REFIID riid, LPVOID* ppvObj)
{
    if (riid == IID_IUnknown || riid == IID_IMMDevice)
    {
        *ppvObj = this;
        return 0;
    }

    *ppvObj = nullptr;
    return E_NOINTERFACE;
}

HRESULT WrappedDevice::Activate(REFIID iid, DWORD dwClsCtx, PROPVARIANT* pActivationParams, void** ppInterface)
{
    auto result = original_->Activate(iid, dwClsCtx, pActivationParams, ppInterface);
    debug("IMMDevice(%p)::Activate(iid=%S) = %#x", this, to_string(iid).c_str(), result);
    return result;
}

HRESULT WrappedDevice::GetId(LPWSTR* ppstrId)
{
    return original_->GetId(ppstrId);
}

HRESULT WrappedDevice::GetState(DWORD* pdwState)
{
    return original_->GetState(pdwState);
}

HRESULT WrappedDevice::OpenPropertyStore(DWORD stgmAccess, IPropertyStore** ppProperties)
{
    return original_->OpenPropertyStore(stgmAccess, ppProperties);
}

WrappedDeviceEnumerator::WrappedDeviceEnumerator(IMMDeviceEnumerator* original) : refcount_(1), original_(original)
{
}

STDMETHODIMP WrappedDeviceEnumerator::QueryInterface(REFIID riid, LPVOID* ppvObj)
{
    if (riid == IID_IUnknown || riid == IID_IMMDeviceEnumerator)
    {
        *ppvObj = this;
        return 0;
    }

    *ppvObj = nullptr;
    return E_NOINTERFACE;
}

HRESULT WrappedDeviceEnumerator::EnumAudioEndpoints(EDataFlow dataFlow, DWORD dwStateMask,
                                                    IMMDeviceCollection** ppDevices)
{
    unimplemented();
}

HRESULT WrappedDeviceEnumerator::GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, IMMDevice** ppEndpoint)
{
    if (selectedAudioDevice)
    {
        debug("IMMDeviceEnumerator::GetDefaultAudioEndpoint overridden");
        selectedAudioDevice->AddRef();
        *ppEndpoint = selectedAudioDevice;
        return S_OK;
    }

    IMMDevice* originalDefault = nullptr;
    HRESULT value = original_->GetDefaultAudioEndpoint(dataFlow, role, &originalDefault);
    if (value != S_OK)
        return value;

    *ppEndpoint = WrappedDeviceRegistry::Instance().FromOriginal(originalDefault);
    return S_OK;
}

HRESULT WrappedDeviceEnumerator::GetDevice(LPCWSTR pwstrId, IMMDevice** ppDevice)
{
    if (!pwstrId || !ppDevice)
    {
        *ppDevice = nullptr;
        return E_POINTER;
    }

    if (selectedAudioDevice)
    {
        if (selectedAudioDeviceId != pwstrId)
        {
            debug("IMMDeviceEnumerator::GetDevice(%S): doesn't match selected device %S", pwstrId,
                  selectedAudioDeviceId.c_str());
            *ppDevice = nullptr;
            return E_NOTFOUND;
        }
        selectedAudioDevice->AddRef();
        *ppDevice = selectedAudioDevice;
        return S_OK;
    }

    IMMDevice* unwrappedDevice = nullptr;
    HRESULT rc = original_->GetDevice(pwstrId, ppDevice);
    if (rc != S_OK)
    {
        return rc;
    }

    *ppDevice = WrappedDeviceRegistry::Instance().FromOriginal(unwrappedDevice);
    return S_OK;
}

HRESULT WrappedDeviceEnumerator::RegisterEndpointNotificationCallback(IMMNotificationClient* pClient)
{
    if (selectedAudioDevice)
    {
        info("IMMDeviceEnumerator::RegisterEndpointNotificationCallback ignored");
        return S_OK;
    }

    return original_->RegisterEndpointNotificationCallback(pClient);
}

HRESULT WrappedDeviceEnumerator::UnregisterEndpointNotificationCallback(IMMNotificationClient* pClient)
{
    if (selectedAudioDevice)
    {
        info("IMMDeviceEnumerator::UnregisterEndpointNotificationCallback ignored");
        return S_OK;
    }

    return original_->UnregisterEndpointNotificationCallback(pClient);
}

static HRESULT CreateIMMDeviceEnumerator(REFCLSID clsid, LPUNKNOWN outer, DWORD clsctx, REFIID iid, LPVOID* ppv)
{
    WrappedDeviceRegistry::Instance().InitializeOnce();
    
    IMMDeviceEnumerator* original = nullptr;
    HRESULT result = OrigCoCreateInstance(clsid, outer, clsctx, iid, reinterpret_cast<void**>(&original));
    if (result != S_OK)
    {
        err("CreateIMMDeviceEnumerator failed: rc = %d", result);
        return result;
    }

    if (!foundOverrideAudioDevice)
    {
        *ppv = original;
        return S_OK;
    }
    
    WrappedDeviceEnumerator* enumerator = new WrappedDeviceEnumerator(original);
    *ppv = enumerator;
    return S_OK;
}

void InitializeAudioHooks()
{
    RegisterCOMHook("IMMDeviceEnumerator", CLSID_MMDeviceEnumerator, IID_IMMDeviceEnumerator,
                    CreateIMMDeviceEnumerator);
}
