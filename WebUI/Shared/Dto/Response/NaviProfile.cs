using WebUI.Shared.Dto.Common;

namespace WebUI.Shared.Dto.Response;

public class NaviProfile
{
    public uint       DefaultUiNaviId     { get; set; }
    public uint       DefaultBattleNaviId { get; set; }
    public List<Navi> UserNavis           { get; set; } = new();
}