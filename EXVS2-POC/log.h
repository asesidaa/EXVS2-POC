#pragma once

#include <stdio.h>

void log(_Printf_format_string_ const char* format, ...);
void fatal [[noreturn]] (_Printf_format_string_ const char* format, ...);