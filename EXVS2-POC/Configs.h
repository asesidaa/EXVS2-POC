#pragma once
#include <optional>
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
};

struct config_struct {
	jvs_key_bind KeyBind = {};
	bool Windowed = false;
	uint8_t Mode = 0;
	std::string InputMode;
	std::string Serial;
	std::string PcbId;
	std::string AuthServerIp;
	std::optional<std::string> InterfaceName;
	std::optional<std::string> IpAddress;
	std::optional<std::string> SubnetMask;
	std::optional<std::string> Gateway;
	std::optional<std::string> TenpoRouter;
	std::optional<std::string> PrimaryDNS;
	std::string ServerAddress;
	std::string RegionCode;

	config_struct() = default;
	config_struct(const config_struct& other) = default;
	config_struct(config_struct&& other) noexcept = default;

	config_struct& operator=(const config_struct& other) = default;
	config_struct& operator=(config_struct&& other) noexcept = default;
};

inline config_struct globalConfig;