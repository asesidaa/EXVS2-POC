#pragma once
#include <optional>
#include <string>
#include <vector>

#include "VirtualKeyMapping.h"

// X macro that enumerates all of the keybinds, along with default keyboard and directinput values
#define KEYBINDS() \
	KEYBIND(Up, "UpArr", "") \
	KEYBIND(Down, "DownArr", "") \
	KEYBIND(Left, "LeftArr", "") \
	KEYBIND(Right, "RightArr", "") \
	KEYBIND(A, "Z", "1") \
	KEYBIND(B, "X", "4") \
	KEYBIND(C, "C", "6") \
	KEYBIND(D, "V", "2") \
	KEYBIND(Start, "1", "10") \
	KEYBIND(Coin, "M", "13") \
	KEYBIND(Card, "P", "7") \
	KEYBIND(Test, "T", "") \
	KEYBIND(Service, "S", "") \
	KEYBIND(Kill, "Esc", "") \

struct KeyBinds {
#define KEYBIND(name, keyboard, dinput) std::vector<int> name;
	KEYBINDS()
#undef KEYBIND
};

enum InputMode {
	// "None"
	InputModeNone = 0x0,

	// "Keyboard"
	InputModeKeyboard = 0x1,

	// "DirectInputOnly"
	InputModeDirectInput = 0x2,

	// "DirectInput"
	InputModeBoth = 0x3,
};

struct config_struct {
	bool Windowed = false;
	uint8_t Mode = 0;

	InputMode InputMode;
	int DirectInputDeviceId;
	KeyBinds DirectInputBindings;
	KeyBinds KeyboardBindings;

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