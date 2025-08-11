// ReSharper disable CppClangTidyClangDiagnosticMicrosoftCast
#define CINTERFACE
#define D3D11_NO_HELPERS
#define INITGUID
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#pragma optimize("", off)

#include "dxgi1_2.h"
#include "dxgi1_3.h"
#include "FramerateLimiter.h"
#include "dxgi1_4.h"
#include "dxgi1_5.h"
#include "dxgi1_6.h"
#include "d3d12.h"
#pragma comment(lib, "d3d12.lib")

#include <intrin.h>

#include "injector.hpp"
#include "MinHook.h"
#include "Configs.h"
#include "log.h"

static IDXGISwapChain* g_swapChain;

// Hook trampolines
static HRESULT(STDMETHODCALLTYPE* g_origSetFullscreenState)(IDXGISwapChain* This, BOOL Fullscreen, IDXGIOutput* pTarget);
static HRESULT(STDMETHODCALLTYPE* g_origCreateSwapChain)(IDXGIFactory2* This, IUnknown* pDevice, DXGI_SWAP_CHAIN_DESC* pDesc, IDXGISwapChain** ppSwapChain);
static HRESULT(WINAPI* g_origCreateDXGIFactory2)(UINT Flags, REFIID riid, void** ppFactory);
static BOOL(WINAPI* g_origSetWindowPos)(HWND hWnd, HWND hWndInsertAfter, int  X, int  Y, int  cx, int  cy, UINT uFlags);
static HWND(WINAPI* g_origCreateWindowExW)(DWORD dwExStyle, LPCWSTR lpClassName, LPCWSTR lpWindowName, DWORD dwStyle, int X, int Y, int nWidth, int nHeight, HWND hWndParent, HMENU hMenu, HINSTANCE hInstance, LPVOID lpParam);
static BOOL(WINAPI* g_origMoveWindow)(HWND hWnd, int X, int Y, int nWidth, int nHeight, BOOL bRepaint);

static HRESULT (STDMETHODCALLTYPE *g_oldPresentWrap) (IDXGISwapChain *pSwapChain, UINT SyncInterval, UINT Flags);

// Functions
template<typename T>
inline T HookVtableFunction(T* functionPtr, T target)
{
	if (*functionPtr == target)
	{
		return nullptr;
	}

	auto old = *functionPtr;
	injector::WriteMemory(functionPtr, target, true);

	return old;
}

static HRESULT STDMETHODCALLTYPE SetFullscreenStateWrap(IDXGISwapChain* This, BOOL Fullscreen, IDXGIOutput* pTarget)
{
	if (!globalConfig.Windowed)
	{
		return g_origSetFullscreenState(This, Fullscreen, pTarget);
	}

	if(globalConfig.Windowed == true && globalConfig.Display.Resolution != "1080p")
	{
		g_origSetFullscreenState(This, true, pTarget);
		g_origSetFullscreenState(This, false, pTarget);
		return S_OK;
	}
	
	return S_OK;
}

static HRESULT STDMETHODCALLTYPE PresentWrap (IDXGISwapChain *pSwapChain, const UINT SyncInterval, const UINT Flags) {
	if (globalConfig.Display.FramerateLimit)
	{
		FramerateLimiter::update();
	}
	
	return g_oldPresentWrap (pSwapChain, SyncInterval, Flags);
}

static HRESULT STDMETHODCALLTYPE CreateSwapChainWrap(IDXGIFactory2* This, IUnknown* pDevice, DXGI_SWAP_CHAIN_DESC* pDesc, IDXGISwapChain** ppSwapChain)
{
	if(globalConfig.Windowed == true && globalConfig.BorderlessWindow == true)
	{
		pDesc->BufferDesc.Scaling = DXGI_MODE_SCALING_STRETCHED;
	}
	
	auto hr = g_origCreateSwapChain(This, pDevice, pDesc, ppSwapChain);
	if (*ppSwapChain)
	{
		auto old = HookVtableFunction(&(*ppSwapChain)->lpVtbl->SetFullscreenState, SetFullscreenStateWrap);
		g_origSetFullscreenState = (old) ? old : g_origSetFullscreenState;

		if (globalConfig.Display.FramerateLimit) {
			const auto old2= HookVtableFunction(&(*ppSwapChain)->lpVtbl->Present, PresentWrap);
			g_oldPresentWrap = old2 ? old2 : g_oldPresentWrap;
		}
	}
	
	g_swapChain = *ppSwapChain;
	return hr;
}

static HRESULT WINAPI CreateDXGIFactory2Wrap(UINT Flags, REFIID riid, void** ppFactory)
{
	HRESULT hr = g_origCreateDXGIFactory2(Flags, riid, ppFactory);

	if (SUCCEEDED(hr))
	{
		const IDXGIFactory2* factory = static_cast<IDXGIFactory2 *> (*ppFactory);
		const auto old = HookVtableFunction(&factory->lpVtbl->CreateSwapChain, CreateSwapChainWrap);
		g_origCreateSwapChain = (old) ? old : g_origCreateSwapChain;
	}
	
	return hr;
}

static HWND WINAPI CreateWindowExWHook(DWORD dwExStyle, LPCWSTR lpClassName, LPCWSTR lpWindowName, DWORD dwStyle, int X, int Y, int nWidth, int nHeight, HWND hWndParent, HMENU hMenu, HINSTANCE hInstance, LPVOID lpParam)
{
	const bool widthHeightGreaterThanZero = nWidth > 0 && nHeight > 0;

	if(!widthHeightGreaterThanZero)
	{
		return g_origCreateWindowExW(dwExStyle, lpClassName, lpWindowName, dwStyle, X, Y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);
	}
	
	if(globalConfig.Windowed == true && globalConfig.BorderlessWindow == true)
	{
		dwStyle = WS_VISIBLE | WS_POPUP | WS_SYSMENU | WS_MINIMIZEBOX;
		X = 0;
		Y = 0;
	}
	else
	{
		dwStyle = WS_VISIBLE | WS_POPUP | WS_CAPTION | WS_SYSMENU | WS_MINIMIZEBOX;
		X = (GetSystemMetrics(SM_CXSCREEN) - nWidth) / 2;
		Y = (GetSystemMetrics(SM_CYSCREEN) - nHeight) / 2;
	}

	// lpWindowName = L"POC";

	return g_origCreateWindowExW(dwExStyle, lpClassName, lpWindowName, dwStyle, X, Y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);
}

static BOOL WINAPI MoveWindowHook(HWND hWnd, int X, int Y, int nWidth, int nHeight, BOOL bRepaint)
{
	if (nWidth > 0 && nHeight > 0)
	{
		X = (GetSystemMetrics(SM_CXSCREEN) - nWidth) / 2;
		Y = (GetSystemMetrics(SM_CYSCREEN) - nHeight) / 2;
		nWidth = 1920;
		nHeight = 1080;
	}

	return g_origMoveWindow(hWnd, X, Y, nWidth, nHeight, bRepaint);
}

static BOOL WINAPI SetWindowPosHook(HWND hWnd, HWND hWndInsertAfter, int X, int Y, int cx, int cy, UINT uFlags)
{
	// Don't put ourselves on top if we're in windowed mode.
	if (hWndInsertAfter == HWND_TOPMOST && g_swapChain)
	{
		DXGI_SWAP_CHAIN_DESC desc;
		HRESULT rc = g_swapChain->lpVtbl->GetDesc(g_swapChain, &desc);
		if (rc != S_OK)
		{
			err("DXGISwapChain::GetDesc failed: %x", rc);
		}
		else if (desc.Windowed)
		{
			hWndInsertAfter = HWND_TOP;
		}
	}
	
	if(globalConfig.Windowed == true && globalConfig.BorderlessWindow == true)
	{
		return g_origSetWindowPos(hWnd, hWndInsertAfter, X, Y, GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN), uFlags);
	}
	
	return g_origSetWindowPos(hWnd, hWndInsertAfter, X, Y, cx, cy, uFlags);
}

void InitDXGIWindowHook()
{
	MH_CreateHookApi(L"user32.dll", "CreateWindowExW", CreateWindowExWHook, reinterpret_cast<void**>(&g_origCreateWindowExW));
	MH_CreateHookApi(L"user32.dll", "MoveWindow", MoveWindowHook, reinterpret_cast<void**>(&g_origMoveWindow));
	MH_CreateHookApi(L"user32.dll", "SetWindowPos", SetWindowPosHook, reinterpret_cast<void**>(&g_origSetWindowPos));
	MH_CreateHookApi(L"dxgi.dll", "CreateDXGIFactory2", CreateDXGIFactory2Wrap, (void**)&g_origCreateDXGIFactory2);

	if (globalConfig.Display.FramerateLimit)
	{
		FramerateLimiter::init();
	}
}
