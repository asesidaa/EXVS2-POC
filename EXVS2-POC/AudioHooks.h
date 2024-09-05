#pragma once

#include <initguid.h>

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
