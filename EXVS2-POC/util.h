#pragma once

#include <string>
#include <vector>

template<typename T>
static std::string to_string(const T& t)
{
    return std::to_string(t);
}

template<>
static std::string to_string(const std::string& t)
{
    return t;
}

template<typename T>
static std::string Join(const std::string& delimiter, const std::vector<T>& vec)
{
    std::string result;
    for (size_t i = 0; i < vec.size(); ++i)
    {
        if (i != 0)
            result += ", ";
        result += to_string(vec[i]);
    }
    return result;
}
