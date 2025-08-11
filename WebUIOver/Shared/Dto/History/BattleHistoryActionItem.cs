namespace WebUIOver.Shared.Dto.History;

public class BattleHistoryActionItem
{
    public required int CardId { get; set; }
    public required string PlayerName { get; set; } = string.Empty;
    public required uint FrameTime { get; set; }
    public required uint ActionType { get; set; }
}