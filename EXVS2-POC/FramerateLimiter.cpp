#include <cstdint>
#include <windows.h>

#include "log.h"

/*
 * Reference: https://github.com/esuo1198/TaikoArcadeLoader/blob/3ce7123685dff41d273012708f389b69c2dd7aec/src/patches/fpslimiter.cpp
 * Reference: https://github.com/teknogods/OpenParrot/blob/master/OpenParrot/src/Functions/FpsLimiter.cpp
 */

namespace FramerateLimiter {
    static LARGE_INTEGER PerformanceCount1;
    static LARGE_INTEGER PerformanceCount2;
    static bool bOnce1 = false;
    static double targetFrameTime = 1000.0 / 60.0;
    static double t = 0.0;
    static std::uint32_t i = 0;

    void init()
    {
        info("Init FPS Limiter");
    }

    void update()
    {
        if (!bOnce1) {
            bOnce1 = true;
            QueryPerformanceCounter (&PerformanceCount1);
            PerformanceCount1.QuadPart = PerformanceCount1.QuadPart >> i;
        }

        while (true) {
            QueryPerformanceCounter (&PerformanceCount2);

            if (t == 0.0) {
                LARGE_INTEGER PerformanceCount3;
                static bool bOnce2 = false;

                if (!bOnce2) {
                    bOnce2 = true;
                    QueryPerformanceFrequency (&PerformanceCount3);
                    i = 0;
                    t = 1000.0 / static_cast<double> (PerformanceCount3.QuadPart);
                    if (auto v = t * 2147483648.0; 60000.0 > v) {
                        while (true) {
                            ++i;
                            v *= 2.0;
                            t *= 2.0;
                            if (60000.0 <= v) break;
                        }
                    }
                }
                SleepEx (0, 1);
                break;
            }

            if (static_cast<double> ((PerformanceCount2.QuadPart >> i) - PerformanceCount1.QuadPart) * t >= targetFrameTime) break;

            SleepEx (0, 1);
        }

        QueryPerformanceCounter (&PerformanceCount2);
        PerformanceCount1.QuadPart = PerformanceCount2.QuadPart >> i;
    }
} // namespace FramerateLimiter
