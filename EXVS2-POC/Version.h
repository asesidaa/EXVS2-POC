#pragma once

#include "log.h"

enum GameVersion
{
    VS2_400 = 4000,
    XBoost_450 = 4050,
    Overboost_480 = 4080,
};

GameVersion GetGameVersion();

#define VS2_XB_OB(vs2, xb, ob)                                                                                         \
    (GetGameVersion() == VS2_400 ? (vs2)                                                                               \
                                    : (GetGameVersion() == XBoost_450 ? (xb)                                           \
                                    : (GetGameVersion() == Overboost_480 ? (ob) : (fatal("unknown game version"), ob))))          

#define XB_OB(xb, ob)                                                                                                  \
    (GetGameVersion() == XBoost_450 ? (xb)                                                                             \
                                    : (GetGameVersion() == Overboost_480 ? ob : (fatal("unknown game version"), ob)))
