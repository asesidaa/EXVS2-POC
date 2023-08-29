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
	int Card;
	int DirectInputDeviceId;
	int ArcadeButton1;
	int ArcadeButton2;
	int ArcadeButton3;
	int ArcadeButton4;
	int ArcadeStartButton;
	int ArcadeCoin;
	int ArcadeTest;
	int ArcadeCard;
	bool UseKeyboardSupportKeyInDirectInput;

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
	    Card(0x50),
		DirectInputDeviceId(16),
		ArcadeButton1(1),
		ArcadeButton2(2),
		ArcadeButton3(3),
		ArcadeButton4(4),
		ArcadeStartButton(5),
		ArcadeCoin(6),
		ArcadeTest(7),
	    ArcadeCard(8),
		UseKeyboardSupportKeyInDirectInput(true)
	{
	}
};

struct config_struct {
	jvs_key_bind KeyBind;
	bool Windowed;
	uint8_t Mode;
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

	config_struct(const config_struct& other) = default;
	config_struct(config_struct&& other) noexcept = default;

	config_struct& operator=(const config_struct& other) = default;
	config_struct& operator=(config_struct&& other) noexcept = default;

	config_struct() :
		Windowed(false),
		Mode(2),
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
		RegionCode("1")
	{
	}
};

inline config_struct globalConfig {};