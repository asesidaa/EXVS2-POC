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

    void Merge(const InputState& rhs)
    {
        if (rhs.X != Direction::Neutral)
            X = rhs.X;
        if (rhs.Y != Direction::Neutral)
            Y = rhs.Y;
        A |= rhs.A;
        B |= rhs.B;
        C |= rhs.C;
        D |= rhs.D;
        Start |= rhs.Start;
        Coin |= rhs.Coin;
        Card |= rhs.Card;
        Test |= rhs.Test;
        Service |= rhs.Service;
        Kill |= rhs.Kill;
    }

    static InputState Get();
};

void InitializeInput();
