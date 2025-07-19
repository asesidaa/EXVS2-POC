using WebUIOver.Shared.Dto.Enum;

namespace WebUIOver.Shared.Dto.Message;

public class CustomMessageGroupSetting
{
    public MessagePosition MessagePosition { get; set; } = MessagePosition.Center;
    public bool AllowReceiveMessage { get; set; } = true;
    public CustomMessageGroup StartGroup { get; set; } = new();
    public CustomMessageGroup InBattleGroup { get; set; } = new();
    public CustomMessageGroup ResultGroup { get; set; } = new();
    public CustomMessageGroup OnlineShuffleStartGroup { get; set; } = new();
    public CustomMessageGroup OnlineShuffleInBattleGroup { get; set; } = new();
    public CustomMessageGroup OnlineShuffleResultGroup { get; set; } = new();
}