#include "VirtualKeyMapping.h"

std::map<int, std::string> keyNames = {
    {0x8, "Backspace"},
    {0x9, "Tab"},
    {0xd, "Return"},
    {0x13, "Pause"},
    {0x20, "Space"},
    {0x21, "PgUp"},
    {0x22, "PgDown"},
    {0x23, "End"},
    {0x24, "Home"},
    {0x25, "LeftArr"},
    {0x26, "UpArr"},
    {0x27, "RightArr"},
    {0x28, "DownArr"},
    {0x2d, "Insert"},
    {0x2e, "Delete"},
    {0x30, "0"},
    {0x31, "1"},
    {0x32, "2"},
    {0x33, "3"},
    {0x34, "4"},
    {0x35, "5"},
    {0x36, "6"},
    {0x37, "7"},
    {0x38, "8"},
    {0x39, "9"},
    {0x41, "A"},
    {0x42, "B"},
    {0x43, "C"},
    {0x44, "D"},
    {0x45, "E"},
    {0x46, "F"},
    {0x47, "G"},
    {0x48, "H"},
    {0x49, "I"},
    {0x4a, "J"},
    {0x4b, "K"},
    {0x4c, "L"},
    {0x4d, "M"},
    {0x4E, "N"},
    {0x4F, "O"},
    {0x50, "P"},
    {0x51, "Q"},
    {0x52, "R"},
    {0x53, "S"},
    {0x54, "T"},
    {0x55, "U"},
    {0x56, "V"},
    {0x57, "W"},
    {0x58, "X"},
    {0x59, "Y"},
    {0x5a, "Z"},
    {0x60, "0 (NumPad)"},
    {0x61, "1 (NumPad)"},
    {0x62, "2 (NumPad)"},
    {0x63, "3 (NumPad)"},
    {0x64, "4 (NumPad)"},
    {0x65, "5 (NumPad)"},
    {0x66, "6 (NumPad)"},
    {0x67, "7 (NumPad)"},
    {0x68, "8 (NumPad)"},
    {0x69, "9 (NumPad)"},
    {0x6a, "* (NumPad)"},
    {0x6b, "+ (NumPad)"},
    {0x6d, "- (NumPad)"},
    {0x6e, ". (NumPad)"},
    {0x6f, "/ (NumPad)"},
    {0x70, "F1"},
    {0x71, "F2"},
    {0x72, "F3"},
    {0x73, "F4"},
    {0x74, "F5"},
    {0x75, "F6"},
    {0x76, "F7"},
    {0x77, "F8"},
    {0x78, "F9"},
    {0x79, "F10"},
    {0x7a, "F11"},
    {0x7b, "F12"},
    {0x7c, "F13"},
    {0x7d, "F14"},
    {0x7e, "F15"},
    {0x7f, "F16"},
    {0x80, "F17"},
    {0x81, "F18"},
    {0x82, "F19"},
    {0x83, "F20"},
    {0x84, "F21"},
    {0x85, "F22"},
    {0x86, "F23"},
    {0x87, "F24"}
};

// Function to find the integer key from the string value using the given map (case-insensitive)
int findKeyByValue(const std::string& value) {
    std::string lowercaseValue = value;
    for (auto& c : lowercaseValue) {
        c = std::tolower(c);
    }

    for (const auto& entry : keyNames) {
        std::string lowercaseEntry = entry.second;
        for (auto& c : lowercaseEntry) {
            c = std::tolower(c);
        }

        if (lowercaseEntry == lowercaseValue) {
            return entry.first;
        }
    }

    return -1; // Return -1 if the value is not found
}