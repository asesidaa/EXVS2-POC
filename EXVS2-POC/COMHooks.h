#pragma once

#include <combaseapi.h>

#include <functional>
#include <optional>
#include <string>

using COMFactory = std::function<HRESULT(REFCLSID, LPUNKNOWN, DWORD clsctx, REFIID, void**)>;

template <typename T> struct retain_ptr
{
  private:
    retain_ptr(T *ptr) : ptr_(ptr)
    {
    }

  public:
    ~retain_ptr()
    {
        reset();
    }

    retain_ptr(const retain_ptr &copy) = delete;
    retain_ptr(retain_ptr &&move)
    {
        this->ptr_ = move.ptr_;
        move.ptr_ = nullptr;
    }

    retain_ptr &operator=(const retain_ptr &copy) = delete;
    retain_ptr &operator=(retain_ptr &&move)
    {
        if (this == &move)
            return;

        reset();
        ptr_ = move.ptr_;
        move.ptr_ = nullptr;
    }

    T *get()
    {
        return ptr_;
    }

    T* operator->()
    {
        return ptr_;
    }

    static retain_ptr AlreadyRetained(T *ptr)
    {
        return retain_ptr(ptr);
    }

    static retain_ptr AddRef(T *ptr)
    {
        ptr->AddRef();
        return retain_ptr(ptr);
    }

  private:
    void reset()
    {
        if (ptr_)
            ptr_->Release();
        ptr_ = nullptr;
    }

    T *ptr_;
};

extern HRESULT (*OrigCoCreateInstance)(REFCLSID clsid, LPUNKNOWN outer, DWORD clsctx, REFIID iid, LPVOID* ppv);

std::u16string to_string(const GUID& guid);

void InitializeCOMHooks();
void RegisterCOMHook(std::string name, std::optional<CLSID> clsid, std::optional<IID> iid, COMFactory factory);

template <typename COMType> void RegisterCOMHook(std::string name, std::optional<CLSID> clsid)
{
    RegisterCOMHook(std::move(name), clsid, std::nullopt, [](REFCLSID, LPUNKNOWN, REFIID iid, void** ppv) {
        auto type = new COMType();
        return type->QueryInterface(iid, ppv);
    });
}
