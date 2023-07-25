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
	char buffer[1024];
	va_list args;
	va_start(args, format);
	char fmt[1024] = "[EXVS2-POC] ";
	strcat_s(fmt, 1024, format);
	vsprintf_s(buffer, 1024, fmt, args);  // NOLINT(cert-err33-c)
	OutputDebugStringA(buffer);
	std::cout << buffer << std::endl;
	va_end(args);
}