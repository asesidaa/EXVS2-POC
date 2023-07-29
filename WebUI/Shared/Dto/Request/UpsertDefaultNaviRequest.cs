namespace WebUI.Shared.Dto.Request;

public class UpsertDefaultNaviRequest : BasicCardRequest
{
    public uint DefaultUiNaviId { get; set; }
    public uint DefaultBattleNaviId { get; set; }
}