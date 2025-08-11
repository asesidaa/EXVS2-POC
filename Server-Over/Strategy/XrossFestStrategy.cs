using nue.protocol.exvs;
using ServerOver.Common.Enum;
using ServerOver.Models.Config;

namespace ServerOver.Strategy;

public class XrossFestStrategy : IFestStrategy
{
    private readonly CardServerConfig _config;
    
    public XrossFestStrategy(CardServerConfig config)
    {
        _config = config;
    }
    
    private readonly uint[] _oneVsOneMsBlackList = { 24, 235, 199, 133, 252 };
    private readonly uint[] _unlimitedBurstMsBlackList = { 139, 162, 165 };
    private readonly uint[] _oneVsOneUnlimitedBurstMsBlackList = { 24, 235, 199, 133, 252, 139, 162, 165 };
    private readonly uint[] _mugenShootingMsBlackList = { 5, 39, 58, 198, 214, 215, 218, 241, 256, 288 };
    
    public Response.LoadGameData.XrossFesSetting determine()
    {
        var currentDateTime = DateTime.Today;

        var ruleType = DetermineRuleType(currentDateTime.DayOfWeek);
        
        DateTimeOffset startDate = DateTime.SpecifyKind(StartOfDay(currentDateTime), DateTimeKind.Local);
        DateTimeOffset endDate = DateTime.SpecifyKind(EndOfDay(currentDateTime), DateTimeKind.Local);
        
        var fesSetting = new Response.LoadGameData.XrossFesSetting
        {
            RuleType = (uint)ruleType,
            StartDate = (ulong)startDate.ToUnixTimeSeconds(),
            EndDate = (ulong)endDate.ToUnixTimeSeconds(),
            BurstXrossFlag = true,
            Timer = 210,
            MatchingBorder = 0,
            MobileSuitBlocklists = Array.Empty<uint>()
        };
        
        if (ruleType is FesType.UnlimitedExBurst)
        {
            fesSetting.MobileSuitBlocklists = _unlimitedBurstMsBlackList;
        }

        if (ruleType is FesType.OneOnOneAllCost or FesType.OneOnOne3000 or FesType.OneOnOne2500 or FesType.OneOnOne2000 or FesType.OneOnOne1500)
        {
            fesSetting.MobileSuitBlocklists = _oneVsOneMsBlackList;
        }
        
        if (ruleType is FesType.OneOnOneUnlimitedExBurstAllCost or FesType.OneOnOneUnlimitedExBurst3000
            or FesType.OneOnOneUnlimitedExBurst2500 or FesType.OneOnOneUnlimitedExBurst2000 
            or FesType.OneOnOneUnlimitedExBurst1500)
        {
            fesSetting.MobileSuitBlocklists = _oneVsOneUnlimitedBurstMsBlackList;
        }

        if (ruleType is FesType.ExMugenShooting)
        {
            fesSetting.MobileSuitBlocklists = _mugenShootingMsBlackList;
        }
        
        if (ruleType is FesType.OneOnOne2000 or FesType.OneOnOne1500
            or FesType.OneOnOneUnlimitedExBurst2000 or FesType.OneOnOneUnlimitedExBurst1500)
        {
            fesSetting.Timer = 240;
        }

        return fesSetting;
    }
    
    private FesType DetermineRuleType(DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek) 
        {
            case DayOfWeek.Sunday:
                return _config.GameConfigurations.FesConfigurations.SundayType;
            case DayOfWeek.Monday:
                return _config.GameConfigurations.FesConfigurations.MondayType;
            case DayOfWeek.Tuesday:
                return _config.GameConfigurations.FesConfigurations.TuesdayType;
            case DayOfWeek.Wednesday:
                return _config.GameConfigurations.FesConfigurations.WednesdayType;
            case DayOfWeek.Thursday:
                return _config.GameConfigurations.FesConfigurations.ThursdayType;
            case DayOfWeek.Friday:
                return _config.GameConfigurations.FesConfigurations.FridayType;
            case DayOfWeek.Saturday:
                return _config.GameConfigurations.FesConfigurations.SaturdayType;
            default:
                return FesType.OneOnOneAllCost;
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