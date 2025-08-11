#include "log.h"
#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING
#include <codecvt>

std::wstring vswprintf_except_it_actually_fucking_works(const char* fmt, va_list args)
{
    std::string utf8;
    utf8.resize(4096);
    size_t len = vsnprintf(utf8.data(), utf8.size(), fmt, args);
    if (len <= utf8.size()) {
        utf8.resize(len);
    }
    return std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>, wchar_t>{}.from_bytes(utf8) + L"\n";
}

void log(LogLevel level, _Printf_format_string_ const char *format, ...)
{
    if (level > g_logLevel)
        return;

    va_list args;
    va_start(args, format);
    std::wstring line = vswprintf_except_it_actually_fucking_works(format, args);
    va_end(args);

    WriteConsoleW(GetStdHandle(STD_OUTPUT_HANDLE), line.c_str(), line.size(), nullptr, nullptr);
}

void fatal [[noreturn]] (_Printf_format_string_ const char *format, ...)
{
    va_list args;
    va_start(args, format);
    std::wstring line = vswprintf_except_it_actually_fucking_works(format, args);
    va_end(args);

    MessageBoxW(nullptr, line.c_str(), L"EXVS2", MB_OK);
    abort();
}
