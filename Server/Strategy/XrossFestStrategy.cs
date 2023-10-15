using nue.protocol.exvs;

namespace Server.Strategy;

public class XrossFestStrategy : IFestStrategy
{
    public Response.LoadGameData.XrossFesSetting determine()
    {
        var currentDateTime = DateTime.Today;

        var ruleType = DetermineRuleType(currentDateTime.DayOfWeek);
        
        DateTimeOffset startDate = DateTime.SpecifyKind(StartOfDay(currentDateTime), DateTimeKind.Local);
        DateTimeOffset endDate = DateTime.SpecifyKind(EndOfDay(currentDateTime), DateTimeKind.Local);
        
        return new Response.LoadGameData.XrossFesSetting
        {
            RuleType = ruleType,
            StartDate = (ulong) startDate.ToUnixTimeSeconds(),
            EndDate = (ulong) endDate.ToUnixTimeSeconds(),
            BurstXrossFlag = true,
            Timer = 420,
            MatchingBorder = 0,
            MobileSuitBlocklists = Array.Empty<uint>()
        };
    }

    private uint DetermineRuleType(DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek) 
        {
            // Sun: Dual Select
            case DayOfWeek.Sunday:
                return 1;
            // Mon: 1 vs 1
            case DayOfWeek.Monday:
                return 0;
            // Tue: Dual Select
            case DayOfWeek.Tuesday:
                return 1;
            // Wed: Score Battle
            case DayOfWeek.Wednesday:
                return 2;
            // Thur: Attack Score Battle
            case DayOfWeek.Thursday:
                return 4;
            // Fri: Attack Boost
            case DayOfWeek.Friday:
                return 5;
            // Sat: 1 vs 1
            case DayOfWeek.Saturday:
                return 0;
            default:
                return 0;
        }
    }
    
    private DateTime EndOfDay(DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
    }

    private DateTime StartOfDay(DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
    }
}