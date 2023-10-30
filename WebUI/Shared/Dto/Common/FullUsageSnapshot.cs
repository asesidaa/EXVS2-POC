namespace WebUI.Shared.Dto.Common;

public class FullUsageSnapshot
{
    public uint CurrentBattleCount { get; set; } = 0;
    public List<FullBurstTypeUsage> FullBurstTypeUsages { get; set; } = new();
    public List<FullMsBattleRecord> FullMsBattleRecords { get; set; } = new();
}