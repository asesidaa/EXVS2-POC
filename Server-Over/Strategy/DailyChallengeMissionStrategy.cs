using ServerOver.Common.Enum;

namespace ServerOver.Strategy;

public class DailyChallengeMissionStrategy
{
    public List<MissionType> DetermineMissionTypes(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Sunday =>
            [
                MissionType.TotalBattleWinCount, MissionType.MaxConsecutiveWinCount, MissionType.TotalDefeatCount
            ],
            DayOfWeek.Monday =>
            [
                MissionType.TotalBattleCount, MissionType.TotalBattleWinCount, MissionType.TotalDefeatCount
            ],
            DayOfWeek.Tuesday =>
            [
                MissionType.TotalBattleCount, MissionType.MaxConsecutiveWinCount, MissionType.TotalDefeatCount
            ],
            DayOfWeek.Wednesday =>
            [
                MissionType.TotalBattleCount, MissionType.TotalBattleWinCount, MissionType.TotalDamageCount
            ],
            DayOfWeek.Thursday =>
            [
                MissionType.TotalBattleCount, MissionType.MaxConsecutiveWinCount, MissionType.TotalDamageCount
            ],
            DayOfWeek.Friday =>
            [
                MissionType.TotalBattleCount, MissionType.TotalBattleWinCount, MissionType.TotalDefeatCount
            ],
            DayOfWeek.Saturday =>
            [
                MissionType.TotalBattleWinCount, MissionType.MaxConsecutiveWinCount, MissionType.TotalDamageCount
            ],
            _ => [MissionType.TotalBattleWinCount, MissionType.MaxConsecutiveWinCount, MissionType.TotalDefeatCount]
        };
    }
}