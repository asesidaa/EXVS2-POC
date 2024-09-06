#pragma once

#include <initguid.h>

#include <Audioclient.h>
#include <Mmdeviceapi.h>

#include <mutex>
#include <thread>
#include <unordered_map>
#include <vector>

#include "COMHooks.h"

void InitializeAudioHooks();

struct WrappedDevice;

struct WrappedDeviceRegistry
{
    WrappedDeviceRegistry() = default;
    WrappedDeviceRegistry(const WrappedDeviceRegistry& copy) = delete;
    WrappedDeviceRegistry(WrappedDeviceRegistry&& move) = delete;
    WrappedDeviceRegistry& operator=(const WrappedDeviceRegistry& copy) = delete;
    WrappedDeviceRegistry& operator=(WrappedDeviceRegistry&& move) = delete;

    void InitializeOnce();

    WrappedDevice* FromOriginal(IMMDevice* originalDevice);
    static WrappedDeviceRegistry& Instance();

    friend struct WrappedDevice;

  private:
    void Remove(IMMDevice* originalDevice);

    std::unordered_map<IMMDevice*, WrappedDevice*> wrapMapping;
    std::mutex mutex_;
    std::once_flag once_;
};

struct WrappedDevice final : public IMMDevice
{
    WrappedDevice(WrappedDeviceRegistry* registry, IMMDevice* original);
    virtual ~WrappedDevice();

    STDMETHODIMP_(ULONG) AddRef() final
    {
        return ++refcount_;
    }

    STDMETHODIMP_(ULONG) Release() final
    {
        auto newRefcount = --refcount_;
        if (newRefcount == 0)
        {
            delete this;
        }
        return newRefcount;
    }

    STDMETHODIMP QueryInterface(REFIID riid, LPVOID* ppvObj) final;
    HRESULT Activate(REFIID iid, DWORD dwClsCtx, PROPVARIANT* pActivationParams, void** ppInterface) final;
    HRESULT GetId(LPWSTR* ppstrId) final;
    HRESULT GetState(DWORD* pdwState) final;
    HRESULT OpenPropertyStore(DWORD stgmAccess, IPropertyStore** ppProperties) final;

  private:
    std::atomic<int> refcount_;
    WrappedDeviceRegistry* registry_;
    IMMDevice* original_;
};

struct WrappedDeviceEnumerator final : public IMMDeviceEnumerator
{
    explicit WrappedDeviceEnumerator(retain_ptr<IMMDeviceEnumerator> original)
        : refcount_(1), original_(std::move(original))
    {
    }

    virtual ~WrappedDeviceEnumerator() = default;

    STDMETHODIMP_(ULONG) AddRef() final
    {
        return ++refcount_;
    }

    STDMETHODIMP_(ULONG) Release() final
    {
        auto newRefcount = --refcount_;
        if (newRefcount == 0)
        {
            delete this;
        }
        return newRefcount;
    }

    STDMETHODIMP QueryInterface(REFIID riid, LPVOID* ppvObj) final;
    HRESULT EnumAudioEndpoints(EDataFlow dataFlow, DWORD dwStateMask, IMMDeviceCollection** ppDevices) final;
    HRESULT GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, IMMDevice** ppEndpoint) final;
    HRESULT GetDevice(LPCWSTR pwstrId, IMMDevice** ppDevice) final;
    HRESULT RegisterEndpointNotificationCallback(IMMNotificationClient* pClient) final;
    HRESULT UnregisterEndpointNotificationCallback(IMMNotificationClient* pClient) final;

  private:
    std::atomic<int> refcount_;
    retain_ptr<IMMDeviceEnumerator> original_;
};

struct WrappedDeviceCollection final : public IMMDeviceCollection
{
    explicit WrappedDeviceCollection(std::vector<retain_ptr<IMMDevice>> devices)
        : refcount_(1), devices_(std::move(devices))
    {
    }
    virtual ~WrappedDeviceCollection() = default;

    STDMETHODIMP_(ULONG) AddRef() final
    {
        return ++refcount_;
    }

    STDMETHODIMP_(ULONG) Release() final
    {
        auto newRefcount = --refcount_;
        if (newRefcount == 0)
        {
            delete this;
        }
        return newRefcount;
    }

    STDMETHODIMP QueryInterface(REFIID riid, LPVOID *ppvObj) final;

    HRESULT GetCount(UINT *pcDevices) final;
    HRESULT Item(UINT nDevice, IMMDevice **ppDevice) final;

  private:
    std::atomic<int> refcount_;
    std::vector<retain_ptr<IMMDevice>> devices_;
};

struct WrappedAudioClient final : public IAudioClient
{
    explicit WrappedAudioClient(retain_ptr<IAudioClient> original) : refcount_(1), original_(std::move(original))
    {
    }
    virtual ~WrappedAudioClient() = default;

    STDMETHODIMP_(ULONG) AddRef() final
    {
        return ++refcount_;
    }

    STDMETHODIMP_(ULONG) Release() final
    {
        auto newRefcount = --refcount_;
        if (newRefcount == 0)
        {
            delete this;
        }
        return newRefcount;
    }

    STDMETHODIMP QueryInterface(REFIID riid, LPVOID *ppvObj) final;

    HRESULT Initialize(AUDCLNT_SHAREMODE ShareMode, DWORD StreamFlags, REFERENCE_TIME hnsBufferDuration,
                       REFERENCE_TIME hnsPeriodicity, const WAVEFORMATEX *pFormat, LPCGUID AudioSessionGuid) final;

    HRESULT GetBufferSize(UINT32 *pNumBufferFrames) final;

    HRESULT GetStreamLatency(REFERENCE_TIME *phnsLatency) final;

    HRESULT GetCurrentPadding(UINT32 *pNumPaddingFrames) final;

    HRESULT IsFormatSupported(AUDCLNT_SHAREMODE ShareMode, const WAVEFORMATEX *pFormat,
                              WAVEFORMATEX **ppClosestMatch) final;

    HRESULT GetMixFormat(WAVEFORMATEX **ppDeviceFormat) final;

    HRESULT GetDevicePeriod(REFERENCE_TIME *phnsDefaultDevicePeriod, REFERENCE_TIME *phnsMinimumDevicePeriod) final;

    HRESULT Start() final;
    HRESULT Stop() final;
    HRESULT Reset() final;

    HRESULT SetEventHandle(HANDLE eventHandle) final;
    HRESULT GetService(REFIID riid, void **ppv) final;

  private:
    std::atomic<int> refcount_;
    retain_ptr<IAudioClient> original_;
};

struct WrappedAudioRenderClient : public IAudioRenderClient
{
    WrappedAudioRenderClient(int id, size_t frameLength, retain_ptr<IAudioRenderClient> original);
    virtual ~WrappedAudioRenderClient() final;

    STDMETHODIMP_(ULONG) AddRef() final
    {
        return ++refcount_;
    }

    STDMETHODIMP_(ULONG) Release() final
    {
        auto newRefcount = --refcount_;
        if (newRefcount == 0)
        {
            delete this;
        }
        return newRefcount;
    }

    STDMETHODIMP QueryInterface(REFIID riid, LPVOID *ppvObj) final;

    HRESULT GetBuffer(UINT32 NumFramesRequested, BYTE **ppData) final;
    HRESULT ReleaseBuffer(UINT32 NumFramesWritten, DWORD dwFlags) final;

  private:
    int id_;
    size_t frameLength_;
    HANDLE file_ = nullptr;
    BYTE *lastBuffer = nullptr;

    std::atomic<int> refcount_;
    retain_ptr<IAudioRenderClient> original_;
};
