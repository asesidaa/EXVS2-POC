namespace WebUI.Shared.Dto.Common;

public class Usage
{
    public Dictionary<uint, uint> BurstTypeUsage { get; set; } = new ();
    public List<MsBattleRecord> MsBattleRecords { get; set; } = new();
}