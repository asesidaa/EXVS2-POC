using ServerOver.Dtos.Live;

namespace ServerOver.Utils;

public static class LiveRankingTimeUtil
{
    public static LiveRankingTime Get()
    {
        var currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
        var currentMonthBegin = new DateTime(currentTime.Year, currentTime.Month, 1, 0, 0, 0);
        var currentMonthEnd = currentMonthBegin.AddMonths(1).AddDays(-1);

        return new LiveRankingTime()
        {
            CurrentTimeStamp = (ulong)((DateTimeOffset)currentTime).ToUnixTimeSeconds(),
            MonthStartTimeStamp = (ulong)((DateTimeOffset)currentMonthBegin).ToUnixTimeSeconds(),
            MonthEndTimeStamp = (ulong)((DateTimeOffset)currentMonthEnd).ToUnixTimeSeconds(),
        };
    }
}