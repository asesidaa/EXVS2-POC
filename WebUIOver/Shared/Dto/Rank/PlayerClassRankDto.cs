namespace WebUIOver.Shared.Dto.Rank;

public class PlayerClassRankDto
{
    public uint Rank { get; set; } = 0;
    public uint ClassId { get; set; } = 0;
    public string PlayerName { get; set; } = string.Empty;
    public uint TopPointRankEntryCount { get; set; } = 0;
    public float ClassRate { get; set; } = 0.0f;
    public uint Percentile { get; set; } = 0;
    public uint PlayerPrestigeId { get; set; } = 0;
    public uint PlayerLevelId { get; set; } = 0;
    public uint TotalWins { get; set; } = 0;
    public uint TotalLosses { get; set; } = 0;
}