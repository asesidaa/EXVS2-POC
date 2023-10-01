#include <Windows.h>

#include "Configs.h"
#include "Input.h"
#include "log.h"

static void PressUp(InputState* out)
{
    out->Y = Direction::Positive;
}

static void PressDown(InputState* out)
{
    out->Y = Direction::Negative;
}

static void PressLeft(InputState* out)
{
    out->X = Direction::Negative;
}

static void PressRight(InputState* out)
{
    out->X = Direction::Positive;
}

#define PRES_BUTAN(name) static void Press ## name(InputState* out) { out->name = true; }
PRES_BUTAN(A)
PRES_BUTAN(B)
PRES_BUTAN(C)
PRES_BUTAN(D)
PRES_BUTAN(Start)
PRES_BUTAN(Coin)
PRES_BUTAN(Card)
PRES_BUTAN(Test)
PRES_BUTAN(Service)
PRES_BUTAN(Kill)
#undef PRES_BUTAN

static void InputStateGetKeyboard(InputState* out)
{
#define KEYBIND(name, kb_default, dinput_default)       \
    for (int key : globalConfig.KeyboardBindings.name)  \
    {                                                   \
        if (GetAsyncKeyState(key) & 0x8000)             \
        {                                               \
            (Press ## name)(out);                       \
            break;                                      \
        }                                               \
    }

    KEYBINDS()
#undef KEYBIND
}

static void InputStateParseDirectInput(InputState* out, JOYINFOEX& joy)
{
    // TODO: Check caps instead of assuming that the joystick ranges from 0-65535.
    double x = (static_cast<int>(joy.dwXpos) - 32767) / 32767.0;
    double y = (static_cast<int>(joy.dwYpos) - 32767) / 32767.0;
    if (x > 0.5) out->X = Direction::Positive;
    else if (x < -0.5) out->X = Direction::Negative;
    if (y > 0.5) out->Y = Direction::Negative;
    else if (y < -0.5) out->Y = Direction::Positive;

    switch (joy.dwPOV)
    {
    case 4500:
    case 9000:
    case 135000:
        out->X = Direction::Positive;
        break;

    case 22500:
    case 27000:
    case 31500:
        out->X = Direction::Negative;
        break;

    default:
        break;
    }

    switch (joy.dwPOV)
    {
    case 0:
    case 4500:
    case 31500:
        out->Y = Direction::Positive;
        break;

    case 13500:
    case 18000:
    case 22500:
        out->Y = Direction::Negative;
        break;

    default:
        break;
    }

    // TODO: Precalculate mask?
#define KEYBIND(name, kb_default, dinput_default)               \
    {                                                           \
        int mask = 0;                                           \
        for (int key : globalConfig.DirectInputBindings.name)   \
            mask |= 1 << (key - 1);                             \
        if (joy.dwButtons & mask) (Press ## name)(out);         \
    }

    KEYBINDS()
#undef KEYBIND
}

static void InputStateGetDirectInput(InputState* out)
{
    JOYINFOEX joy;
    joy.dwSize = sizeof(joy);
    joy.dwFlags = JOY_RETURNBUTTONS | JOY_RETURNCENTERED | JOY_RETURNX | JOY_RETURNY;

    int deviceId = globalConfig.DirectInputDeviceId;
    if (deviceId < 16)
    {
        if (joyGetPosEx(deviceId, &joy) != JOYERR_NOERROR)
        {
            err("failed to get input for DirectInput device %d", deviceId);
            return;
        }
        InputStateParseDirectInput(out, joy);
        return;
    }

    for (UINT joystickIndex = 0; joystickIndex < 16; ++joystickIndex)
    {
        if (joyGetPosEx(joystickIndex, &joy) == JOYERR_NOERROR)
        {
            InputStateParseDirectInput(out, joy);
        }
    }
}

InputState InputState::Get()
{
    InputState result;

    if (globalConfig.InputMode & InputModeKeyboard)
        InputStateGetKeyboard(&result);

    if (globalConfig.InputMode & InputModeDirectInput)
        InputStateGetDirectInput(&result);

    return result;
}