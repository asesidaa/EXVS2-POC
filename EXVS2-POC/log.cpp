#include "log.h"
#include <stdio.h>
#include <cstdarg>
#include <cstdlib>
#include <cstring>

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <iostream>

void log(const char* format, ...)
{
#ifdef _DEBUG
	char buffer[1024];
	va_list args;
	va_start(args, format);
	char fmt[1024] = "[EXVS2-POC] ";
	strcat_s(fmt, 1024, format);
	vsprintf_s(buffer, 1024, fmt, args);  // NOLINT(cert-err33-c)
	OutputDebugStringA(buffer);
	std::cout << buffer << std::endl;
	va_end(args);
#endif
}

void fatal [[noreturn]] (_Printf_format_string_ const char* format, ...)
{
	char buf[4096];
	va_list args;
	va_start(args, format);
	vsnprintf(buf, sizeof(buf), format, args);
	va_end(args);

	MessageBoxA(nullptr, buf, "EXVS2-POC", MB_OK);
	abort();
}