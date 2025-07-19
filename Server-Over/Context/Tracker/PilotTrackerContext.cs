namespace ServerOver.Context.Tracker;

public class PilotTrackerContext
{
    public uint TotalBattleCount { get; set; } = 0;
    public uint TotalWinCount { get; set; } = 0;
    public uint TotalGivenDamage { get; set; } = 0;
    public uint TotalEnemyDefeatedCount { get; set; } = 0;
    public uint TotalClassMatchTenConsecutiveWinCount { get; set; } = 0;
    public uint TotalNoDamageBattleCount { get; set; } = 0;
    public uint TotalExBurstDamage { get; set; } = 0;
    public uint PlayerLevel { get; set; } = 0;
    public float SoloClassRate { get; set; } = 0.0f;
    public float TeamClassRate { get; set; } = 0.0f;
}