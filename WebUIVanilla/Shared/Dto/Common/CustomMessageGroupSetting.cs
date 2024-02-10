namespace WebUIVanilla.Shared.Dto.Common;

public class CustomMessageGroupSetting
{
    public CustomMessageGroup? StartGroup { get; set; } = new();
    public CustomMessageGroup? InBattleGroup { get; set; } = new();
    public CustomMessageGroup? ResultGroup { get; set; } = new();
    public CustomMessageGroup? OnlineShuffleStartGroup { get; set; } = new();
    public CustomMessageGroup? OnlineShuffleInBattleGroup { get; set; } = new();
    public CustomMessageGroup? OnlineShuffleResultGroup { get; set; } = new();
}