namespace WebUI.Shared.Dto.Common;

public class CustomMessageGroupSetting
{
    public CustomMessageGroup? StartGroup { get; set; } = new();
    public CustomMessageGroup? InBattleGroup { get; set; } = new();
    public CustomMessageGroup? ResultGroup { get; set; } = new();
}