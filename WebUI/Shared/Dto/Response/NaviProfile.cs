using WebUI.Shared.Dto.Common;

namespace Server.Dto.Response;

public class NaviProfile
{
    public uint defaultUiNaviId { get; set; }
    public uint defaultBattleNaviId { get; set; }
    public List<Navi> userNavis { get; set; }
}