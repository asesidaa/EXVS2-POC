#pragma once

#include <combaseapi.h>

#include <functional>
#include <optional>
#include <string>

using COMFactory = std::function<HRESULT(REFCLSID, LPUNKNOWN, DWORD clsctx, REFIID, void**)>;

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