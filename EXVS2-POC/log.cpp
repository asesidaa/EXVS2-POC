#include "log.h"
#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

void log(LogLevel level, _Printf_format_string_ const char* format, ...)
{
	if (level > g_logLevel) return;

	va_list args;
	va_start(args, format);
	char fmt[1024] = "[EXVS2-POC] ";
	strcat_s(fmt, format);
	strcat_s(fmt, "\n");
	vprintf_s(fmt, args);  // NOLINT(cert-err33-c)
	va_end(args);
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