namespace ServerOver.Context.Battle.Domain.PvP;

public class BattleStatisticDomain
{
    public ulong PlayedAt { get; set; } = 0;
    public uint ElapsedSeconds { get; set; } = 0;
    public uint StageId { get; set; } = 0;
    public uint Score { get; set; } = 0;
    public uint ScoreRank { get; set; } = 0;
    public uint TotalGivenDamage { get; set; } = 0;
    public uint TotalTakenDamage { get; set; } = 0;
    public uint TotalEnemyDefeatedCount { get; set; } = 0;
    public uint ConsecutiveWinCount { get; set; } = 0;
    public bool NoDamageFlag { get; set; } = false;
    public uint TotalExBurstDamage { get; set; } = 0;
    public uint BurstType { get; set; } = 0;
    public uint BurstCount { get; set; } = 0;
    public uint OverheatCount { get; set; } = 0;
    public uint ComboGivenDamage { get; set; } = 0;
    public uint SkinId { get; set; } = 0;
}