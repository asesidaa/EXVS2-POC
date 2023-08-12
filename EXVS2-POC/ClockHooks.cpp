#include <Windows.h>
#include <cstdint>

#include "MinHook.h"
#include "log.h"

#include "ClockHooks.h"

#include <chrono>
#include <ctime>

// Reference https://github.com/djhackersdev/segatools/blob/ca9c72db968c81fdf88ba01f9b4a474bf818e401/platform/clock.c

constexpr auto jiffies_per_sec = 10000000LL;
constexpr auto jiffies_per_hour = jiffies_per_sec * 3600LL;
constexpr auto jiffies_per_day = jiffies_per_hour * 24LL;

static int64_t clock_current_day;

static void (WINAPI *GetSystemTimeAsFileTimeOri)(FILETIME *out);
static void WINAPI GetSystemTimeAsFileTimeHook(FILETIME *out)
{
    log("GetSystemTimeAsFileTimeHook");
    FILETIME in;

    if (out == nullptr) {
        SetLastError(ERROR_INVALID_PARAMETER);

        return;
    }

    /* Get and convert real jiffies */

    GetSystemTimeAsFileTimeOri(&in);
    int64_t real_jiffies = (((int64_t)in.dwHighDateTime) << 32) | in.dwLowDateTime;

    /* Keepout period is JST [02:00, 07:00), which is equivalent to
       UTC [17:00, 22:00). Bias UTC forward by 2 hours, changing this interval
       to [19:00, 00:00) to make the math easier. We revert this bias later. */

    int64_t real_jiffies_biased = real_jiffies + 2LL * jiffies_per_hour;

    /* Split date and time */

    int64_t day = real_jiffies_biased / jiffies_per_day;
    int64_t real_time = real_jiffies_biased % jiffies_per_day;

    /* Debug log */

    if (clock_current_day != 0 && clock_current_day != day) {
        log("\n*** CLOCK JUMP! ***\n\n");
    }

    clock_current_day = day;

    /* We want to skip the final five hours of our UTC+2 biased reference frame,
       so scale time-of-day by 19/24. */

    int64_t fake_time = (real_time * 19LL) / 24LL;

    /* Un-split date and time */

    int64_t fake_jiffies_biased = day * jiffies_per_day + fake_time;

    /* Revert bias */

    int64_t fake_jiffies = fake_jiffies_biased - 2LL * jiffies_per_hour;

    /* Return result */

    out->dwLowDateTime  = fake_jiffies & 0xFFFFFFFF;
    out->dwHighDateTime = fake_jiffies >> 32;
}

static BOOL (WINAPI *GetLocalTimeOri)(SYSTEMTIME *out);
static BOOL WINAPI GetLocalTimeHook(SYSTEMTIME *out)
{
    ULARGE_INTEGER arith;
    FILETIME linear;

    /* Force JST */

    GetSystemTimeAsFileTimeHook(&linear);

    arith.LowPart = linear.dwLowDateTime;
    arith.HighPart = linear.dwHighDateTime;
    arith.QuadPart += 9ULL * jiffies_per_hour;
    linear.dwLowDateTime = arith.LowPart;
    linear.dwHighDateTime = arith.HighPart;

    return FileTimeToSystemTime(&linear, out);
}

static BOOL (WINAPI *GetSystemTimeOri)(SYSTEMTIME *out);
static BOOL WINAPI GetSystemTimeHook(SYSTEMTIME *out)
{
    FILETIME linear;

    GetSystemTimeAsFileTimeHook(&linear);
    BOOL ok = FileTimeToSystemTime(&linear, out);

    if (!ok)
    {
        return ok;
    }
    
    return TRUE;
}

static DWORD (WINAPI *GetTimeZoneInformationOri)(TIME_ZONE_INFORMATION *tzinfo);
static DWORD WINAPI GetTimeZoneInformationHook(TIME_ZONE_INFORMATION *tzinfo)
{
    log("Clock: Returning JST timezone\n");

    if (tzinfo == nullptr) {
        SetLastError(ERROR_INVALID_PARAMETER);

        return TIME_ZONE_ID_INVALID;
    }

    /* Force JST (UTC+9), SEGA games malfunction in any other time zone.
       Strings and boundary times don't matter, we only set the offset. */

    memset(tzinfo, 0, sizeof(*tzinfo));
    tzinfo->Bias = -9 * 60;

    SetLastError(ERROR_SUCCESS);

    /* "Unknown" here means that this region does not observe DST */

    return TIME_ZONE_ID_UNKNOWN;
}

static __time64_t (*time64Ori)( __time64_t *destTime );
static __time64_t time64Hook(time_t* destTime)
{
    using namespace std::literals;
    using namespace std::chrono;
    
    auto now =
        system_clock::now();

    zoned_time jpZonedTime{"Asia/Tokyo", now};
    auto time_point = floor<days>(jpZonedTime.get_local_time());
    auto today = time_point;
    
    const auto notBefore = today + 1h + 45min;
    const auto notAfter = today + 7h;

    if (jpZonedTime.get_local_time() >= notBefore &&
        jpZonedTime.get_local_time() <= notAfter)
    {
        now += {5h};
    }

    auto ret = duration_cast<seconds>(now.time_since_epoch()).count();
    if (destTime != nullptr)
    {
        *destTime = ret;
    }
    return ret;
}

void InitClockHooks()
{
    MH_Initialize();

    MH_CreateHookApi(L"kernel32.dll", "GetSystemTimeAsFileTime", GetSystemTimeAsFileTimeHook, reinterpret_cast<void**>(&GetSystemTimeAsFileTimeOri));
    MH_CreateHookApi(L"kernel32.dll", "GetLocalTime", GetLocalTimeHook, reinterpret_cast<void**>(&GetLocalTimeOri));
    MH_CreateHookApi(L"kernel32.dll", "GetSystemTime", GetSystemTimeHook, reinterpret_cast<void**>(&GetSystemTimeOri));
    MH_CreateHookApi(L"kernel32.dll", "GetTimeZoneInformation", GetTimeZoneInformationHook, reinterpret_cast<void**>(&GetTimeZoneInformationOri));
    MH_CreateHookApi(L"api-ms-win-crt-time-l1-1-0.dll", "_time64", time64Hook, reinterpret_cast<void**>(&time64Ori));
    
    MH_EnableHook(nullptr);
}


