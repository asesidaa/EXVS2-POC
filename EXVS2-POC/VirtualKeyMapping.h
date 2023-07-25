#pragma once
#ifndef KEYNAMES_H
#define KEYNAMES_H

#include <map>
#include <string>

extern std::map<int, std::string> keyNames;

int findKeyByValue(const std::string& value);

#endif // KEYNAMES_H