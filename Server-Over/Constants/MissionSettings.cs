using ServerOver.Common.Enum;
using ServerOver.Common.Structure;

namespace ServerOver.Constants;

public class MissionSettings
{
    private static readonly MissionSetting TotalBattleCount = new () {
        Id = 1,
        DesiredNum = 10
    };

    private static readonly MissionSetting TotalBattleWinCount = new () {
        Id = 2,
        DesiredNum = 5
    };

    private static readonly MissionSetting MaxConsecutiveWinCount = new () {
        Id = 3,
        DesiredNum = 3
    };

    private static readonly MissionSetting TotalDefeatCount = new () {
        Id = 4,
        DesiredNum = 10
    };

    private static readonly MissionSetting TotalDamageCount = new () {
        Id = 5,
        DesiredNum = 5000
    };

    public static MissionSetting GetBy(MissionType missionType)
    {
        return missionType switch
        {
            MissionType.TotalBattleCount => TotalBattleCount,
            MissionType.TotalBattleWinCount => TotalBattleWinCount,
            MissionType.MaxConsecutiveWinCount => MaxConsecutiveWinCount,
            MissionType.TotalDefeatCount => TotalDefeatCount,
            MissionType.TotalDamageCount => TotalDamageCount,
            _ => TotalBattleCount
        };
    }
}