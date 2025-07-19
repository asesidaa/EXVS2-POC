namespace WebUIOver.Shared.Dto.Request;

public class UpsertNaviProfileRequest : BasicCardRequest
{
    public uint DefaultUiNaviId { get; set; }
    public uint DefaultBattleNaviId { get; set; }
    public bool BattleNavAdviseFlag { get; set; } = true;
    public bool BattleNavNotifyFlag { get; set; } = true;
}