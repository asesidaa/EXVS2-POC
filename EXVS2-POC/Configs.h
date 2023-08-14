#pragma once
#include <string>
#include "VirtualKeyMapping.h"

struct jvs_key_bind {
	int KillProcess;
	int Test;
	int Start;
	int Service;
	int Coin;
	int Up;
	int Left;
	int Down;
	int Right;
	int Button1;
	int Button2;
	int Button3;
	int Button4;
	int DirectInputDeviceId;
	int ArcadeButton1;
	int ArcadeButton2;
	int ArcadeButton3;
	int ArcadeButton4;
	int ArcadeStartButton;
	int ArcadeCoin;
	int ArcadeTest;

	jvs_key_bind():
		KillProcess(0x1B),
		Test(0x54),
		Start(0x4F),
		Service(0x53),
		Coin(0x4D),
		Up(0x26),
		Left(0x25),
		Down(0x28),
		Right(0x27),
		Button1(0x5A),
		Button2(0x58),
		Button3(0x43),
		Button4(0x56),
		DirectInputDeviceId(16),
		ArcadeButton1(1),
		ArcadeButton2(2),
		ArcadeButton3(3),
		ArcadeButton4(4),
		ArcadeStartButton(5),
		ArcadeCoin(6),
		ArcadeTest(7)
	{}
};

struct config_struct {
	jvs_key_bind KeyBind;
	bool Windowed;
	std::string Mode;
	std::string InputMode;
	std::string Serial;
	std::string PcbId;
	std::string TenpoRouter;
	std::string AuthServerIp;
	std::string IpAddress;
	std::string SubnetMask;
	std::string Gateway;
	std::string PrimaryDNS;
	std::string ServerAddress;
	std::string RegionCode;
	bool UseNormalTimeInLM;

	config_struct(const config_struct& other) = default;

	config_struct(config_struct&& other) noexcept
		: KeyBind(other.KeyBind),
		  Windowed(other.Windowed),
		  Mode(std::move(other.Mode)),
		  InputMode(std::move(other.InputMode)),
		  Serial(std::move(other.Serial)),
		  PcbId(std::move(other.PcbId)),
		  TenpoRouter(std::move(other.TenpoRouter)),
		  AuthServerIp(std::move(other.AuthServerIp)),
		  IpAddress(std::move(other.IpAddress)),
		  SubnetMask(std::move(other.SubnetMask)),
		  Gateway(std::move(other.Gateway)),
		  PrimaryDNS(std::move(other.PrimaryDNS)),
		  ServerAddress(std::move(other.ServerAddress)),
		  RegionCode(std::move(other.RegionCode)),
	      UseNormalTimeInLM(other.UseNormalTimeInLM)
	{
	}

	config_struct& operator=(const config_struct& other)
	{
		if (this == &other)
			return *this;
		KeyBind = other.KeyBind;
		Windowed = other.Windowed;
		Mode = other.Mode;
		InputMode = other.InputMode;
		Serial = other.Serial;
		PcbId = other.PcbId;
		TenpoRouter = other.TenpoRouter;
		AuthServerIp = other.AuthServerIp;
		IpAddress = other.IpAddress;
		SubnetMask = other.SubnetMask;
		Gateway = other.Gateway;
		PrimaryDNS = other.PrimaryDNS;
		ServerAddress = other.ServerAddress;
		RegionCode = other.RegionCode;
		UseNormalTimeInLM = other.UseNormalTimeInLM;
		return *this;
	}

	config_struct& operator=(config_struct&& other) noexcept
	{
		if (this == &other)
			return *this;
		KeyBind = other.KeyBind;
		Windowed = other.Windowed;
		Mode = std::move(other.Mode);
		InputMode = std::move(other.InputMode);
		Serial = std::move(other.Serial);
		PcbId = std::move(other.PcbId);
		TenpoRouter = std::move(other.TenpoRouter);
		AuthServerIp = std::move(other.AuthServerIp);
		IpAddress = std::move(other.IpAddress);
		SubnetMask = std::move(other.SubnetMask);
		Gateway = std::move(other.Gateway);
		PrimaryDNS = std::move(other.PrimaryDNS);
		ServerAddress = std::move(other.ServerAddress);
		RegionCode = std::move(other.RegionCode);
		UseNormalTimeInLM = other.UseNormalTimeInLM;
		return *this;
	}

	config_struct() :
		Windowed(false),
		Mode("2"),
		InputMode("Keyboard"),
		Serial("284311110001"),
		PcbId("ABLN1110001"),
		TenpoRouter("192.168.1.1"),
		AuthServerIp("127.0.0.1"),
		IpAddress("192.168.1.2"),
		SubnetMask("255.255.255.0"),
		Gateway("192.168.1.1"),
		PrimaryDNS("8.8.8.8"),
		ServerAddress("127.0.0.1"),
		RegionCode("1"),
		UseNormalTimeInLM(false)
	{
	}
};