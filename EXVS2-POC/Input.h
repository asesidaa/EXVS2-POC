#pragma once

#include <stdint.h>

enum class Direction : int8_t
{
    Positive = 1,
    Neutral = 0,
    Negative = -1,
};

struct InputState
{
    Direction X = Direction::Neutral;
    Direction Y = Direction::Neutral;

    bool A = false;
    bool B = false;
    bool C = false;
    bool D = false;

    bool Start = false;
    bool Coin = false;
    bool Card = false;
    bool Test = false;
    bool Service = false;

    bool Kill = false;

    static InputState Get();
};