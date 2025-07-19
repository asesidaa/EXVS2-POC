using WebUIOver.Shared.Dto.History.Stats;

namespace WebUIOver.Shared.Dto.History;

public class BattleHistorySummary
{
    public required int BattleHistoryId { get; set; }
    public required ulong PlayedAt { get; set; }
    public required bool IsWin { get; set; }
    public required uint ElapsedSeconds { get; set; }
    public required uint StageId { get; set; }
    public required uint Score { get; set; }
    public required uint BurstType { get; set; }
    public required BattleHistoryPlayer SelfPlayer { get; set; }
    public BattleHistoryPlayer? Teammate { get; set; }
    public required List<BattleHistoryPlayer> Opponents { get; set; } = new();
    public required List<BattleHistoryActionItem> ActionItems { get; set; } = new();
    public required MiscStats MiscStats { get; set; }
    public required DamageStats DamageStats { get; set; }
    public required BurstStats BurstStats { get; set; }
}