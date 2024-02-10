namespace ServerVanilla.Context.Battle.Domain.PvP;

public class BattleStatisticDomain
{
    public uint TotalGivenDamage { get; set; } = 0;
    public uint TotalEnemyDefeatedCount { get; set; } = 0;
    public uint ConsecutiveWinCount { get; set; } = 0;
    public bool NoDamageFlag { get; set; } = false;
    public uint TotalExBurstDamage { get; set; } = 0;
}