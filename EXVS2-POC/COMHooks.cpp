#include "COMHooks.h"

#include <Windows.h>

#include <string>

#include "MinHook.h"

#include "log.h"

struct COMHook
{
    std::string name;
    std::optional<CLSID> clsid;
    std::optional<IID> iid;
    COMFactory factory;
};

static std::vector<COMHook> hooks;
HRESULT (*OrigCoCreateInstance)(REFCLSID clsid, LPUNKNOWN outer, DWORD clsctx, REFIID iid, LPVOID* ppv) = nullptr;

std::u16string to_string(const GUID& guid)
{
    std::u16string result;
    result.resize(38);

    int rc = StringFromGUID2(guid, reinterpret_cast<wchar_t*>(result.data()), result.size() + 1);
    if (rc == 0)
        abort();

    return result;
}

static HRESULT CoCreateInstanceHook(REFCLSID clsid, LPUNKNOWN outer, DWORD clsctx, REFIID iid, LPVOID* ppv)
{
    for (const auto& hook : hooks)
    {
        if (hook.clsid && clsid != hook.clsid)
            continue;
        if (hook.iid && iid != hook.iid)
            continue;
        debug("CoCreateInstance(%s) hooked: clsid=%S, outer=%p, clsctx=%#X, iid=%S", hook.name.c_str(),
              to_string(clsid).c_str(), outer, clsctx, to_string(iid).c_str());
        return hook.factory(clsid, outer, clsctx, iid, ppv);
    }

    debug("CoCreateInstance unhooked: clsid=%S, outer=%p, clsctx=%#X, iid=%S", to_string(clsid).c_str(), outer, clsctx,
          to_string(iid).c_str());
    return OrigCoCreateInstance(clsid, outer, clsctx, iid, ppv);
}

void InitializeCOMHooks()
{
    info("COM hooks initialized");
    MH_CreateHookApi(L"combase.dll", "CoCreateInstance", CoCreateInstanceHook,
                     reinterpret_cast<void**>(&OrigCoCreateInstance));
}

void RegisterCOMHook(std::string name, std::optional<CLSID> clsid, std::optional<IID> iid, COMFactory factory)
{
    hooks.emplace_back(COMHook{
        .name = std::move(name),
        .clsid = clsid,
        .iid = iid,
        .factory = factory,
    });
}
