namespace WebUIOver.Shared.Dto.History;

public class BattleHistoryPlayer
{
    public required uint CardId { get; set; }
    public required string PlayerName { get; set; }
    public bool HasCard { get; set; }
    public uint BurstType { get; set; }
    public required uint ClassId { get; set; }
    public required uint PrestigeId { get; set; }
    public required uint LevelId { get; set; }
    public required uint MobileSuitId { get; set; }
    public uint CostumeId { get; set; } = 0;
    public required uint SkinId { get; set; }
    public required uint Mastery { get; set; }
    public uint ConsecutiveWinCount { get; set; } = 0;
}