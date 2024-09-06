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

static const IID IID_IMMDeviceCollection = __uuidof(IMMDeviceCollection);
static const IID IID_IAudioClient = __uuidof(IAudioClient);

DEFINE_GUID(IID_IMMDevice, 0xd666063f, 0x1587, 0x4e43, 0x81, 0xf1, 0xb9, 0x48, 0xe8, 0x07, 0x36, 0x3f);

static WrappedDevice* selectedAudioDevice;
static std::wstring selectedAudioDeviceId;

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

static void DumpAudioDevices()
{
    HRESULT rc;
    IMMDeviceEnumerator* enumerator;

    rc = OrigCoCreateInstance(CLSID_MMDeviceEnumerator, nullptr, CLSCTX_ALL, IID_IMMDeviceEnumerator,
                              reinterpret_cast<void**>(&enumerator));
    if (rc != S_OK)
    {
        fatal("failed to create IMMDeviceEnumerator: %#x", rc);
    }

    IMMDeviceCollection *rawDevices;
    rc = enumerator->EnumAudioEndpoints(eRender, DEVICE_STATE_ACTIVE, &rawDevices);
    if (rc != S_OK)
    {
        fatal("failed to enumerate audio endpoints: %#x", rc);
    }
    auto devices = retain_ptr<IMMDeviceCollection>::AlreadyRetained(rawDevices);

    unsigned int count;
    rc = devices->GetCount(&count);
    if (rc != S_OK)
    {
        fatal("failed to get audio device count: %#x", rc);
    }

    for (unsigned int i = 0; i < count; ++i)
    {
        IMMDevice *device;
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

        bool selected = selectedAudioDeviceId == deviceId;
        CoTaskMemFree(deviceId);

        DumpDevice(i, deviceId, device, selected);
        device->Release();
    }
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
        fatal("failed to create IMMDeviceEnumerator: %#x", rc);
    }

    IMMDeviceCollection *rawDevices;
    rc = enumerator->EnumAudioEndpoints(eRender, DEVICE_STATE_ACTIVE, &rawDevices);
    if (rc != S_OK)
    {
        fatal("failed to enumerate audio endpoints: %#x", rc);
    }

    auto devices = retain_ptr<IMMDeviceCollection>::AlreadyRetained(rawDevices);

    unsigned int count;
    rc = devices->GetCount(&count);
    if (rc != S_OK)
    {
        fatal("failed to get audio device count: %#x", rc);
    }

    for (unsigned int i = 0; i < count; ++i)
    {
        IMMDevice *device;
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

        bool selected = expectedDeviceId ? _wcsicmp(deviceId, expectedDeviceId->c_str()) == 0 : false;
        CoTaskMemFree(deviceId);

        if (selected)
        {
            result = device;
        }
        else
        {
            device->Release();
        }
    }

    if (!result)
    {
        if (expectedDeviceId)
        {
            warn("Failed to find audio device, returning default audio endpoint");
        }
        else
        {
            info("No device specified, returning default audio endpoint");
        }

        rc = enumerator->GetDefaultAudioEndpoint(eRender, eConsole, &result);
        if (rc != S_OK)
        {
            err("GetDefaultAudioEndpoint failed: %#x", rc);
        }
    }

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

        if (IMMDevice* device = FindAudioDevice(deviceId))
        {
            selectedAudioDevice = FromOriginal(device);

            LPWSTR retrievedDeviceId;
            if (device->GetId(&retrievedDeviceId) != S_OK)
            {
                fatal("failed to get IMMDevice id");
            }
            selectedAudioDeviceId = retrievedDeviceId;
            CoTaskMemFree(retrievedDeviceId);
        }

        DumpAudioDevices();
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
    if (!ppvObj)
        return E_POINTER;

    if (riid == IID_IUnknown || riid == IID_IMMDevice)
    {
        *ppvObj = this;
        return 0;
    }

    err("WrappedDevice::QueryInterface for unsupported interface: %S", to_string(riid).c_str());
    *ppvObj = nullptr;
    return E_NOINTERFACE;
}

HRESULT WrappedDevice::Activate(REFIID iid, DWORD dwClsCtx, PROPVARIANT* pActivationParams, void** ppInterface)
{
    if (!ppInterface)
        return E_POINTER;

    HRESULT result;
    if (iid == IID_IAudioClient)
    {
        IAudioClient *audioClient = nullptr;
        result = original_->Activate(iid, dwClsCtx, pActivationParams, reinterpret_cast<void **>(&audioClient));
        if (result != 0)
        {
            err("IMMDevice(%p)::Activate(IAudioClient, %#x) failed: %#x", this, dwClsCtx, result);
            return result;
        }

        debug("IMMDevice(%p)::Activate(IAudioClient, %#x) returning WrappedAudioClient");
        *ppInterface = new WrappedAudioClient(retain_ptr<IAudioClient>::AlreadyRetained(audioClient));
        return S_OK;
    }

    result = original_->Activate(iid, dwClsCtx, pActivationParams, ppInterface);
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


STDMETHODIMP WrappedDeviceEnumerator::QueryInterface(REFIID riid, LPVOID* ppvObj)
{
    if (!ppvObj)
        return E_POINTER;

    if (riid == IID_IUnknown || riid == IID_IMMDeviceEnumerator)
    {
        *ppvObj = this;
        return 0;
    }

    err("WrappedDeviceEnumerator::QueryInterface for unsupported interface: %S", to_string(riid).c_str());
    *ppvObj = nullptr;
    return E_NOINTERFACE;
}

HRESULT WrappedDeviceEnumerator::EnumAudioEndpoints(EDataFlow dataFlow, DWORD dwStateMask,
                                                    IMMDeviceCollection** ppDevices)
{
    if (!ppDevices)
        return E_POINTER;

    std::vector<retain_ptr<IMMDevice>> collection;

    if ((dataFlow == eRender || dataFlow == eAll) && (dwStateMask & DEVICE_STATE_ACTIVE))
    {
        if (selectedAudioDevice)
        {
            collection.emplace_back(retain_ptr<IMMDevice>::AddRef(selectedAudioDevice));
        }
    }

    *ppDevices = new WrappedDeviceCollection(std::move(collection));
    return S_OK;
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

    WrappedDeviceEnumerator *enumerator =
        new WrappedDeviceEnumerator(retain_ptr<IMMDeviceEnumerator>::AlreadyRetained(original));
    *ppv = enumerator;
    return S_OK;
}

STDMETHODIMP WrappedDeviceCollection::QueryInterface(REFIID riid, LPVOID *ppvObj)
{
    if (!ppvObj)
        return E_POINTER;

    if (riid == IID_IUnknown || riid == IID_IMMDeviceCollection)
    {
        *ppvObj = this;
        return 0;
    }

    err("WrappedDeviceCollection::QueryInterface for unsupported interface: %S", to_string(riid).c_str());
    *ppvObj = nullptr;
    return E_NOINTERFACE;
}

HRESULT WrappedDeviceCollection::GetCount(UINT *pcDevices)
{
    if (!pcDevices)
        return E_POINTER;
    *pcDevices = devices_.size();
    return S_OK;
}

HRESULT WrappedDeviceCollection::Item(UINT nDevice, IMMDevice **ppDevice)
{
    if (!ppDevice)
        return E_POINTER;
    if (nDevice >= devices_.size())
        return E_INVALIDARG;

    IMMDevice *result = devices_[nDevice].get();
    result->AddRef();
    *ppDevice = result;
    return S_OK;
}

STDMETHODIMP WrappedAudioClient::QueryInterface(REFIID riid, LPVOID *ppvObj)
{
    if (!ppvObj)
        return E_POINTER;

    if (riid == IID_IUnknown || riid == IID_IAudioClient)
    {
        *ppvObj = this;
        return 0;
    }

    err("WrappedAudioClient::QueryInterface for unsupported interface: %S", to_string(riid).c_str());
    *ppvObj = nullptr;
    return E_NOINTERFACE;
}

HRESULT WrappedAudioClient::Initialize(AUDCLNT_SHAREMODE ShareMode, DWORD StreamFlags, REFERENCE_TIME hnsBufferDuration,
                                       REFERENCE_TIME hnsPeriodicity, const WAVEFORMATEX *pFormat,
                                       LPCGUID AudioSessionGuid)
{
    HRESULT result =
        original_->Initialize(ShareMode, StreamFlags, hnsBufferDuration, hnsPeriodicity, pFormat, AudioSessionGuid);
    debug("WrappedAudioClient::Initialize = %#x", result);
    return result;
}

HRESULT WrappedAudioClient::GetBufferSize(UINT32 *pNumBufferFrames)
{
    HRESULT result = original_->GetBufferSize(pNumBufferFrames);
    debug("WrappedAudioClient::GetBufferSize = %#x", result);
    return result;
}

HRESULT WrappedAudioClient::GetStreamLatency(REFERENCE_TIME *phnsLatency)
{
    HRESULT result = original_->GetStreamLatency(phnsLatency);
    debug("WrappedAudioClient::GetStreamLatency = %#x", result);
    return result;
}

HRESULT WrappedAudioClient::GetCurrentPadding(UINT32 *pNumPaddingFrames)
{
    HRESULT result = original_->GetCurrentPadding(pNumPaddingFrames);
    debug("WrappedAudioClient::GetCurrentPadding = %#x", result);
    return result;
}

HRESULT WrappedAudioClient::IsFormatSupported(AUDCLNT_SHAREMODE ShareMode, const WAVEFORMATEX *pFormat,
                                              WAVEFORMATEX **ppClosestMatch)
{
    HRESULT result = original_->IsFormatSupported(ShareMode, pFormat, ppClosestMatch);
    debug("WrappedAudioClient::IsFormatSupported = %#x", result);
    return result;
}

HRESULT WrappedAudioClient::GetMixFormat(WAVEFORMATEX **ppDeviceFormat)
{
    if (!ppDeviceFormat)
        return E_POINTER;

    HRESULT result = original_->GetMixFormat(ppDeviceFormat);
    debug("WrappedAudioClient::GetMixFormat = %#x", result);
    info("IAudioClient::GetMixFormat reported %d audio channels", (*ppDeviceFormat)->nChannels);
    return result;
}

HRESULT WrappedAudioClient::GetDevicePeriod(REFERENCE_TIME *phnsDefaultDevicePeriod,
                                            REFERENCE_TIME *phnsMinimumDevicePeriod)
{
    HRESULT result = original_->GetDevicePeriod(phnsDefaultDevicePeriod, phnsMinimumDevicePeriod);
    debug("WrappedAudioClient::GetDevicePeriod = %#x", result);
    return result;
}

HRESULT WrappedAudioClient::Start()
{
    HRESULT result = original_->Start();
    debug("WrappedAudioClient::Start = %#x", result);
    return result;
}

HRESULT WrappedAudioClient::Stop()
{
    HRESULT result = original_->Stop();
    debug("WrappedAudioClient::Stop = %#x", result);
    return result;
}
HRESULT WrappedAudioClient::Reset()
{
    HRESULT result = original_->Reset();
    debug("WrappedAudioClient::Reset = %#x", result);
    return result;
}

HRESULT WrappedAudioClient::SetEventHandle(HANDLE eventHandle)
{
    HRESULT result = original_->SetEventHandle(eventHandle);
    debug("WrappedAudioClient::SetEventHandle = %#x", result);
    return result;
}

HRESULT WrappedAudioClient::GetService(REFIID riid, void **ppv)
{
    HRESULT result = original_->GetService(riid, ppv);
    debug("WrappedAudioClient::GetService = %#x", result);
    return result;
}

void InitializeAudioHooks()
{
    RegisterCOMHook("IMMDeviceEnumerator", CLSID_MMDeviceEnumerator, IID_IMMDeviceEnumerator,
                    CreateIMMDeviceEnumerator);
}
