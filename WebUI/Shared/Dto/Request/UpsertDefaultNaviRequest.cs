namespace WebUI.Shared.Dto.Request;

public class UpsertDefaultNaviRequest : BasicCardRequest
{
    public uint defaultUiNaviId { get; set; }
    public uint defaultBattleNaviId { get; set; }
}