#pragma once

#include "log.h"

enum GameVersion
{
    VS2_400 = 4000,
    XBoost_450 = 4050
};

GameVersion GetGameVersion();

#define VS2_XB(vs2, xb)                                                                                                  \
    (GetGameVersion() == XBoost_450 ? (xb)                                                                             \
                                    : (GetGameVersion() == VS2_400 ? vs2 : (fatal("unknown game version"), vs2)))
