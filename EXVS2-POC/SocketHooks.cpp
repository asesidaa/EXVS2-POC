#include "SocketHooks.h"

#include <stdio.h>
#include <stdlib.h>
#include <winsock2.h>
#include <ws2ipdef.h>
#include <ws2tcpip.h>
#include <iphlpapi.h>

#pragma comment(lib, "ws2_32.lib")
#pragma comment(lib, "iphlpapi.lib")

#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING
#include <codecvt>
#include <string>
#include <vector>

#include "MinHook.h"

#include "Configs.h"
#include "log.h"

static struct
{
	std::string guid;
	std::string name;
	std::vector<BYTE> macAddress;

	std::string ipAddress;
	std::vector<char> ipSockaddr;

	std::string gateway;
	std::vector<char> gatewaySockaddr;

	std::string subnetMask;
	uint8_t prefixLength;
	std::vector<char> prefixSockaddr;
} selected;

static const char* SocketTypeToString(int type)
{
	switch (type)
	{
	case SOCK_DGRAM:
		return "SOCK_DGRAM";
	case SOCK_STREAM:
		return "SOCK_STREAM";
	default:
		return "<unknown>";
	}
}

static std::optional<std::string> SockaddrToString(const sockaddr* addr, bool includePort = false)
{
	std::optional<std::string> result;
	int port = 0;
	if (addr->sa_family == AF_INET)
	{
		result.emplace();
		result->resize(INET_ADDRSTRLEN);

		auto sockaddr = reinterpret_cast<LPSOCKADDR_IN>(const_cast<struct sockaddr*>(addr));
		port = sockaddr->sin_port;
		inet_ntop(addr->sa_family, &sockaddr->sin_addr, result->data(), result->size());
	}
	else if (addr->sa_family == AF_INET6)
	{
		result.emplace();
		result->resize(INET_ADDRSTRLEN);

		auto sockaddr = reinterpret_cast<LPSOCKADDR_IN6>(const_cast<struct sockaddr*>(addr));
		port = sockaddr->sin6_port;
		inet_ntop(addr->sa_family, &sockaddr->sin6_addr, result->data(), result->size());
	}
	else
	{
		return {};
	}

	result->resize(strlen(result->data()));
	if (includePort)
	{
		*result += ':';
		*result += std::to_string(port);
	}
	return result;
}

template<typename F>
static bool IterateInterfaces(F&& callback) {
	DWORD rc = 0;

	std::vector<char> buffer;
	PIP_ADAPTER_ADDRESSES pAddresses = nullptr;
	ULONG outBufLen = 16384;

	for (int i = 0; i < 32; ++i)
	{
		buffer.resize(outBufLen);
		pAddresses = reinterpret_cast<PIP_ADAPTER_ADDRESSES>(buffer.data());

		rc = GetAdaptersAddresses(AF_INET, GAA_FLAG_INCLUDE_PREFIX | GAA_FLAG_INCLUDE_GATEWAYS, nullptr, pAddresses, &outBufLen);
		if (rc != ERROR_BUFFER_OVERFLOW)
		{
			break;
		}
	}

	if (rc != NO_ERROR)
	{
		fatal("Failed to enumerate network adapters");
	}

	for (PIP_ADAPTER_ADDRESSES cur = pAddresses; cur->Next; cur = cur->Next)
	{
		std::u16string currentInterfaceNameU16 = reinterpret_cast<char16_t*>(cur->FriendlyName);
		std::string currentInterfaceName = std::wstring_convert<
			std::codecvt_utf8_utf16<char16_t>, char16_t>{}.to_bytes(currentInterfaceNameU16);

		if (callback(std::move(currentInterfaceName), cur))
		{
			return true;
		}
	}
	return false;
}

std::string Ipv4PrefixLengthToMask(int prefix)
{
	std::string buf;
	for (int i = 0; i < 4; ++i)
	{
		int bits = prefix > 8 ? 8 : prefix;
		int mask = (1 << bits) - 1;
		buf += std::to_string(mask);
		buf += '.';
		prefix -= bits;
	}
	buf.pop_back();

	printf("Subnet mask: %s\n", buf.c_str());
	return buf;
}

static bool SelectInterface(std::string* error, PIP_ADAPTER_ADDRESSES adapter, std::string&& interfaceName, const std::optional<std::string>& expectedIp = std::nullopt)
{
	std::optional<std::string> v4AddrString;
	std::string v4GatewayString = "0.0.0.0";
	std::optional<std::string> subnetMask;

	std::string v4DnsString = "0.0.0.0";

	for (PIP_ADAPTER_UNICAST_ADDRESS unicast = adapter->FirstUnicastAddress; unicast; unicast = unicast->Next)
	{
		LPSOCKADDR addr = unicast->Address.lpSockaddr;
		if (addr->sa_family != AF_INET)
			continue;

		v4AddrString = SockaddrToString(addr);

		// Select the first IPv4 address, unless we're expecting a specific IP.
		if (expectedIp && expectedIp != v4AddrString)
		{
			continue;
		}

		if (v4AddrString)
		{
			const char* p = reinterpret_cast<char*>(addr);
			selected.ipSockaddr.assign(p, p + unicast->Address.iSockaddrLength);
			break;
		}
	}

	for (PIP_ADAPTER_GATEWAY_ADDRESS gateway = adapter->FirstGatewayAddress; gateway; gateway = gateway->Next)
	{
		LPSOCKADDR addr = gateway->Address.lpSockaddr;
		if (addr->sa_family != AF_INET)
		{
			printf("Skipping non-ipv4 gateway\n");
			continue;
		}

		// Always select the first IPv4 gateway.
		if (std::optional<std::string> opt = SockaddrToString(addr); opt)
		{
			v4GatewayString = *opt;

			const char* p = reinterpret_cast<char*>(addr);
			selected.gatewaySockaddr.assign(p, p + gateway->Address.iSockaddrLength);
			break;
		}
	}

	for (PIP_ADAPTER_DNS_SERVER_ADDRESS dns = adapter->FirstDnsServerAddress; dns; dns = dns->Next)
	{
		LPSOCKADDR addr = dns->Address.lpSockaddr;
		if (addr->sa_family != AF_INET)
			continue;

		if (std::optional<std::string> opt = SockaddrToString(addr); opt)
		{
			v4DnsString = *opt;
			break;
		}
	}

	for (PIP_ADAPTER_PREFIX prefix = adapter->FirstPrefix; prefix; prefix = prefix->Next)
	{
		LPSOCKADDR addr = prefix->Address.lpSockaddr;
		if (addr->sa_family != AF_INET)
		{
			printf("Skipping non-ipv4 prefix\n");
			continue;
		}

		const char* p = reinterpret_cast<char*>(addr);
		selected.prefixSockaddr.assign(p, p + prefix->Address.iSockaddrLength);
		selected.prefixLength = static_cast<uint8_t>(prefix->PrefixLength);
		subnetMask = Ipv4PrefixLengthToMask(prefix->PrefixLength);
		break;
	}

#define BAIL(...) do { if (error) { char buf[4096]; snprintf(buf, sizeof(buf), __VA_ARGS__); *error = buf; } return false; } while (0)
	if (!v4AddrString)
		BAIL("Failed to find IPv4 address for interface %s", interfaceName.c_str());
	if (!subnetMask)
		BAIL("Failed to find subnet mask for interface %s", interfaceName.c_str());
#undef BAIL

	printf("Selected interface %s:\n", interfaceName.c_str());
	printf("  Address: %s\n", v4AddrString->c_str());
	printf("  Gateway: %s\n", v4GatewayString.c_str());
	printf("  Subnet mask: %s\n", subnetMask->c_str());
	printf("  Description: %S\n", adapter->Description);

	selected.name = interfaceName;
	selected.guid = adapter->AdapterName;
	selected.macAddress.assign(adapter->PhysicalAddress, adapter->PhysicalAddress + adapter->PhysicalAddressLength);
	selected.ipAddress = *v4AddrString;
	selected.gateway = v4GatewayString;
	selected.subnetMask = *subnetMask;

#define SET_IF_UNSET(lhs, rhs) do { if (!(lhs)) { printf("Setting %s to %s\n", #lhs, (rhs).c_str()); lhs.emplace(rhs); } else { printf("Skipping %s, already set to %s\n", #lhs, lhs->c_str()); } } while (0)
	SET_IF_UNSET(globalConfig.InterfaceName, std::move(interfaceName));
	SET_IF_UNSET(globalConfig.IpAddress, std::move(*v4AddrString));
	SET_IF_UNSET(globalConfig.Gateway, v4GatewayString);
	SET_IF_UNSET(globalConfig.TenpoRouter, std::move(v4GatewayString));
	SET_IF_UNSET(globalConfig.SubnetMask, std::move(*subnetMask));
	SET_IF_UNSET(globalConfig.PrimaryDNS, std::move(v4DnsString));
#undef SET_IF_UNSET

	return true;
}

static bool FindInterfaceByName(const std::string& interfaceName)
{
	printf("Selecting interface by name: %s\n", interfaceName.c_str());
	return IterateInterfaces(
		[&](std::string&& currentInterfaceName, PIP_ADAPTER_ADDRESSES adapter)
		{
			std::string error;
			printf("Current interface: %s\n", currentInterfaceName.c_str());

			if (interfaceName == currentInterfaceName)
			{
				// Exact match, abort on failure.
				if (!SelectInterface(&error, adapter, std::move(currentInterfaceName)))
					fatal("%s", error.c_str());

				return true;
			}
			else if (currentInterfaceName.starts_with(interfaceName))
			{
				// Fuzzy match, let failures go by.
				if (SelectInterface(nullptr, adapter, std::move(currentInterfaceName)))
					return true;
			}

			return false;
		}
	);
}

static bool FindInterfaceByAddress(const std::string& ipAddress)
{
	printf("Selecting interface by address: %s\n", ipAddress.c_str());
	return IterateInterfaces(
		[&](std::string&& currentInterfaceName, PIP_ADAPTER_ADDRESSES adapter)
		{
			for (PIP_ADAPTER_UNICAST_ADDRESS unicast = adapter->FirstUnicastAddress; unicast; unicast = unicast->Next) {
				LPSOCKADDR addr = unicast->Address.lpSockaddr;
				std::optional<std::string> addrString = SockaddrToString(addr);
				if (!addrString)
					continue;
				if (*addrString == ipAddress)
				{
					std::string error;
					if (!SelectInterface(&error, adapter, std::move(currentInterfaceName), std::move(addrString)))
						fatal("%s", error.c_str());
					return true;
				}
			}
			return false;
		}
	);
}

static void FindInterface()
{
	if (globalConfig.InterfaceName && globalConfig.IpAddress)
		fatal("Only one of InterfaceName and IpAddress can be specified in config.ini");

	bool result = false;
	if (globalConfig.InterfaceName)
		result = FindInterfaceByName(*globalConfig.InterfaceName);
	else if (globalConfig.IpAddress)
		result = FindInterfaceByAddress(*globalConfig.IpAddress);
	else
		fatal("At least one of InterfaceName or IpAddress should be specified in config.ini");

	if (!result)
		fatal("Failed to find interface");
}

static SOCKET(WSAAPI* orig_WSASocketW)(int af, int type, int protocol, LPWSAPROTOCOL_INFOW lpProtocolInfo, GROUP g, DWORD dwFlags);
static SOCKET WSAAPI WSASocketWHook(int af, int type, int protocol, LPWSAPROTOCOL_INFOW lpProtocolInfo, GROUP g, DWORD dwFlags)
{
	SOCKET result = orig_WSASocketW(af, type, protocol, lpProtocolInfo, g, dwFlags);
	printf("WSASocketW(%d, %s, %d, ?, ?, %d) = %lld\n", af, SocketTypeToString(type), protocol, dwFlags, result);
	return result;
}

static int (WSAAPI* orig_bind)(SOCKET s, const struct sockaddr* name, int namelen);
static int bindHook(SOCKET s, const struct sockaddr* name, int namelen)
{
	if (name->sa_family == AF_INET6)
	{
		int rc = orig_bind(s, name, namelen);
		const struct sockaddr_in6* origAddr = reinterpret_cast<const struct sockaddr_in6*>(name);
		printf("bind(%lld, %s) = %d\n", s, SockaddrToString(name, true)->c_str(), rc);
		return rc;
	}

	const struct sockaddr_in* origAddr = reinterpret_cast<const struct sockaddr_in*>(name);
	int port = origAddr->sin_port;

	std::vector<char> addrBuf = selected.ipSockaddr;
	struct sockaddr_in* addr = reinterpret_cast<struct sockaddr_in*>(addrBuf.data());
	addr->sin_port = port;

	int rc = orig_bind(s, reinterpret_cast<sockaddr*>(addrBuf.data()), static_cast<int>(addrBuf.size()));
	printf("bind(%lld): %s -> %s = %d\n", s, SockaddrToString(name, true)->c_str(), SockaddrToString(reinterpret_cast<sockaddr*>(addr), true)->c_str(), rc);
	return rc;
}

struct BumpAllocator
{
	BumpAllocator(void* p, size_t length) : base(static_cast<char*>(p)), size(length)
	{
		memset(base, 0, size);
	}

	void* Allocate(size_t p)
	{
		if (p > Available())
			return nullptr;

		char* result = base + used;
		used += p;

		// Align.
		constexpr size_t alignment = 16;
		size_t extra = used % alignment;
		if (extra != 0)
			used += alignment - extra;

		return result;
	}

	template<typename T>
	T* Allocate()
	{
		return static_cast<T*>(Allocate(sizeof(T)));
	}

	template<typename T>
	T* Allocate(size_t length)
	{
		return static_cast<T*>(Allocate(sizeof(T) * length));
	}

	size_t Available() const
	{
		return size - used;
	}

	char* base;
	size_t used = 0;
	size_t size;
};

static ULONG (WINAPI* orig_GetAdaptersInfo)(PIP_ADAPTER_INFO AdapterInfo, PULONG SizePointer);
static ULONG WINAPI GetAdaptersInfoHook(PIP_ADAPTER_INFO AdapterInfo, PULONG SizePointer)
{
	BumpAllocator alloc(AdapterInfo, *SizePointer);
	PIP_ADAPTER_INFO adapter = alloc.Allocate<IP_ADAPTER_INFO>();
	adapter->Next = nullptr;
	adapter->ComboIndex = 0;
	sprintf_s(adapter->AdapterName, "%s", selected.guid.c_str());
	sprintf_s(adapter->Description, "%s", selected.name.c_str());
	memcpy(adapter->Address, selected.macAddress.data(), selected.macAddress.size());
	adapter->AddressLength = static_cast<unsigned int>(selected.macAddress.size());
	adapter->Index = 0;
	adapter->Type = MIB_IF_TYPE_ETHERNET;
	adapter->DhcpEnabled = 0;
	adapter->CurrentIpAddress = &adapter->IpAddressList;

	adapter->IpAddressList.Next = nullptr;
	strcpy_s(adapter->IpAddressList.IpAddress.String, selected.ipAddress.c_str());
	strcpy_s(adapter->IpAddressList.IpMask.String, selected.subnetMask.c_str());
	adapter->IpAddressList.Context = 0;

	adapter->GatewayList.Next = nullptr;
	strcpy_s(adapter->GatewayList.IpAddress.String, selected.gateway.c_str());
	adapter->GatewayList.Context = 0;

	adapter->HaveWins = false;

	return NO_ERROR;
}

static ULONG (WINAPI* orig_GetAdaptersAddresses)(ULONG Family, ULONG Flags, PVOID Reserved, PIP_ADAPTER_ADDRESSES AdapterAddresses, PULONG SizePointer);
static ULONG WINAPI GetAdaptersAddressesHook(ULONG Family, ULONG Flags, PVOID Reserved, PIP_ADAPTER_ADDRESSES AdapterAddresses, PULONG SizePointer)
{
	if (!AdapterAddresses)
	{
		*SizePointer = 16384;
		return ERROR_BUFFER_OVERFLOW;
	}

	BumpAllocator alloc(AdapterAddresses, *SizePointer);
	PIP_ADAPTER_ADDRESSES adapter = alloc.Allocate<IP_ADAPTER_ADDRESSES>();
	adapter->Length = sizeof(IP_ADAPTER_ADDRESSES);
	adapter->IfIndex = 0;
	adapter->Next = nullptr;
	adapter->AdapterName = alloc.Allocate<char>(selected.guid.size() + 1);
	snprintf(adapter->AdapterName, selected.guid.size() + 1, "%s", selected.guid.c_str());

	adapter->FirstUnicastAddress = alloc.Allocate<IP_ADAPTER_UNICAST_ADDRESS>();
	adapter->FirstUnicastAddress->Length = sizeof(IP_ADAPTER_UNICAST_ADDRESS);
	adapter->FirstUnicastAddress->Next = nullptr;
	adapter->FirstUnicastAddress->Address.lpSockaddr = alloc.Allocate<SOCKADDR>(selected.ipSockaddr.size());
	memcpy(adapter->FirstUnicastAddress->Address.lpSockaddr, selected.ipSockaddr.data(), selected.ipSockaddr.size());
	adapter->FirstUnicastAddress->Address.iSockaddrLength = static_cast<int>(selected.ipSockaddr.size());
	adapter->FirstUnicastAddress->Flags = 0;
	adapter->FirstUnicastAddress->PrefixOrigin = IpPrefixOriginManual;
	adapter->FirstUnicastAddress->SuffixOrigin = IpSuffixOriginManual;
	adapter->FirstUnicastAddress->DadState = IpDadStatePreferred;
	adapter->FirstUnicastAddress->ValidLifetime = 0xffffffff;
	adapter->FirstUnicastAddress->PreferredLifetime = 0xffffffff;
	adapter->FirstUnicastAddress->LeaseLifetime = 0xffffffff;
	adapter->FirstUnicastAddress->OnLinkPrefixLength = selected.prefixLength;

	adapter->FirstAnycastAddress = nullptr;
	adapter->FirstMulticastAddress = nullptr;
	adapter->FirstDnsServerAddress = nullptr;

	adapter->Description = const_cast<wchar_t*>(L"Ethernet Adapter");
	adapter->FriendlyName = alloc.Allocate<wchar_t>((selected.name.size() + 1) * 2);
	swprintf_s(adapter->FriendlyName, selected.name.size() + 1, L"%hs", selected.name.c_str());

	memcpy(adapter->PhysicalAddress, selected.macAddress.data(), selected.macAddress.size());
	adapter->PhysicalAddressLength = static_cast<ULONG>(selected.macAddress.size());

	adapter->Ipv4Enabled = true;
	adapter->Mtu = 1500;
	adapter->IfType = IF_TYPE_ETHERNET_CSMACD;
	adapter->OperStatus = IfOperStatusUp;

	adapter->FirstPrefix = alloc.Allocate<IP_ADAPTER_PREFIX_XP>();
	adapter->FirstPrefix->Length = sizeof(IP_ADAPTER_PREFIX_XP);
	adapter->FirstPrefix->Next = nullptr;
	adapter->FirstPrefix->Address.lpSockaddr = alloc.Allocate<SOCKADDR>(selected.prefixSockaddr.size());
	memcpy(adapter->FirstPrefix->Address.lpSockaddr, selected.prefixSockaddr.data(), selected.prefixSockaddr.size());
	adapter->FirstPrefix->Address.iSockaddrLength = static_cast<int>(selected.prefixSockaddr.size());
	adapter->FirstPrefix->PrefixLength = selected.prefixLength;

	if (selected.gatewaySockaddr.size() > 0)
	{
		adapter->FirstGatewayAddress = alloc.Allocate<IP_ADAPTER_GATEWAY_ADDRESS>();
		adapter->FirstGatewayAddress->Length = sizeof(IP_ADAPTER_GATEWAY_ADDRESS);
		adapter->FirstGatewayAddress->Next = nullptr;
		adapter->FirstGatewayAddress->Address.lpSockaddr = alloc.Allocate<SOCKADDR>(selected.gatewaySockaddr.size());
		memcpy(adapter->FirstGatewayAddress->Address.lpSockaddr, selected.gatewaySockaddr.data(), selected.gatewaySockaddr.size());
		adapter->FirstGatewayAddress->Address.iSockaddrLength = static_cast<int>(selected.gatewaySockaddr.size());
	}

	adapter->ConnectionType = NET_IF_CONNECTION_DEDICATED;

	return NO_ERROR;
}

void InitializeSocketHooks()
{
	FindInterface();
	MH_CreateHookApi(L"WS2_32.dll", "WSASocketW", WSASocketWHook, reinterpret_cast<void**>(&orig_WSASocketW));
	MH_CreateHookApi(L"WS2_32.dll", "bind", bindHook, reinterpret_cast<void**>(&orig_bind));
	MH_CreateHookApi(L"IPHLPAPI.dll", "GetAdaptersInfo", GetAdaptersInfoHook, reinterpret_cast<void**>(&orig_GetAdaptersInfo));
	MH_CreateHookApi(L"IPHLPAPI.dll", "GetAdaptersAddresses", GetAdaptersAddressesHook, reinterpret_cast<void**>(&orig_GetAdaptersAddresses));
}
