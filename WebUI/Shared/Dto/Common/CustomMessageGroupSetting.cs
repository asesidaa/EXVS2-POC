using WebUI.Shared.Dto.Enum;

namespace WebUI.Shared.Dto.Common;

public class CustomMessageGroupSetting
{
    public MessagePosition MessagePosition { get; set; } = MessagePosition.Center;
    public CustomMessageGroup? StartGroup { get; set; } = new();
    public CustomMessageGroup? InBattleGroup { get; set; } = new();
    public CustomMessageGroup? ResultGroup { get; set; } = new();
}