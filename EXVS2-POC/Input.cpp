#include <Windows.h>

#include <atomic>
#include <memory>

#include "Configs.h"
#include "Input.h"
#include "log.h"

static std::atomic<std::shared_ptr<InputConfig>> g_currentInputConfig;

void UpdateInputConfig(InputConfig&& newConfig)
{
    auto config = std::make_shared<InputConfig>(std::move(newConfig));
    info("Updating input config:\n%s", config->Dump("  ").c_str());
    g_currentInputConfig = std::move(config);
}

static void PressUp(InputState* out)
{
    out->Y = Direction::Negative;
}

static void PressDown(InputState* out)
{
    out->Y = Direction::Positive;
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

static void InputStateGetKeyboard(InputState* out, InputConfig* config)
{
#define KEYBIND(name, kb_default, dinput_default)                                                                      \
    for (int key : config->KeyboardBindings.name)                                                                      \
    {                                                                                                                  \
        if (GetAsyncKeyState(key) & 0x8000)                                                                            \
        {                                                                                                              \
            (Press##name)(out);                                                                                        \
            break;                                                                                                     \
        }                                                                                                              \
    }

    KEYBINDS()
#undef KEYBIND
}

static void InputStateParseDirectInput(InputState* out, InputConfig* config, JOYINFOEX& joy)
{
    // TODO: Check caps instead of assuming that the joystick ranges from 0-65535.
    double x = (static_cast<int>(joy.dwXpos) - 32767) / 32767.0;
    double y = (static_cast<int>(joy.dwYpos) - 32767) / 32767.0;
    if (x > 0.5) out->X = Direction::Positive;
    else if (x < -0.5) out->X = Direction::Negative;
    if (y > 0.5) out->Y = Direction::Positive;
    else if (y < -0.5) out->Y = Direction::Negative;

    switch (joy.dwPOV)
    {
    case 4500:
    case 9000:
    case 13500:
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
        out->Y = Direction::Negative;
        break;

    case 13500:
    case 18000:
    case 22500:
        out->Y = Direction::Positive;
        break;

    default:
        break;
    }

    // TODO: Precalculate mask?
#define KEYBIND(name, kb_default, dinput_default)                                                                      \
    {                                                                                                                  \
        int mask = 0;                                                                                                  \
        for (int key : config->ControllerBindings.name)                                                                \
            mask |= 1 << (key - 1);                                                                                    \
        if (joy.dwButtons & mask)                                                                                      \
            (Press##name)(out);                                                                                        \
    }

    KEYBINDS()
#undef KEYBIND
}

static void InputStateGetDirectInput(InputState* out, InputConfig* config)
{
    static bool previousResult = true;
    JOYINFOEX joy = {};
    joy.dwSize = sizeof(joy);
    joy.dwFlags = JOY_RETURNBUTTONS | JOY_RETURNCENTERED | JOY_RETURNX | JOY_RETURNY;

    int deviceId = config->ControllerDeviceId;
    if (deviceId >= 0 && deviceId < 16)
    {
        if (joyGetPosEx(deviceId, &joy) != JOYERR_NOERROR)
        {
            if (previousResult) {
              err("failed to get input for DirectInput device %d", deviceId);
              previousResult = false;
            }
            return;
        }
        previousResult = true;
        InputStateParseDirectInput(out, config, joy);
        return;
    }

    bool scanAll = deviceId < 0;
    for (UINT joystickIndex = 0; joystickIndex < 16; ++joystickIndex)
    {
        bool success = joyGetPosEx(joystickIndex, &joy) == JOYERR_NOERROR;
        if (success)
        {
            InputStateParseDirectInput(out, config, joy);
        }

        if (success && !scanAll)
        {
            break;
        }
    }
}

InputState InputState::Get()
{
    InputState result;
    std::shared_ptr<InputConfig> config = g_currentInputConfig;

    if (config->KeyboardEnabled)
        InputStateGetKeyboard(&result, config.get());

    if (config->ControllerEnabled)
        InputStateGetDirectInput(&result, config.get());

    return result;
}
