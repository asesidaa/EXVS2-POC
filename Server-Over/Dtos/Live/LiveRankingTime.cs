namespace ServerOver.Dtos.Live;

public class LiveRankingTime
{
    public ulong CurrentTimeStamp { get; set; } = 0;
    public ulong MonthStartTimeStamp { get; set; } = 0;
    public ulong MonthEndTimeStamp { get; set; } = 0;
}