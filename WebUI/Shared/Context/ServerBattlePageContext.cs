using WebUI.Shared.Dto.Common;

namespace WebUI.Shared.Context;

public class ServerBattlePageContext
{
    public Usage UsageStat { get; set; } = new();
    public List<MsBattleRecord> MsBattleRecords { get; set; } = new();
    public Dictionary<uint, uint> CostUsage { get; set; } = new();
    public double TotalBattleCount { get; set; } = 0;
    public double TotalWinCount { get; set; } = 0;
}