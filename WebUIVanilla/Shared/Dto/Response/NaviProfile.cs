using WebUIVanilla.Shared.Dto.Common;

namespace WebUIVanilla.Shared.Dto.Response;

public class NaviProfile
{
    public uint DefaultUiNaviId { get; set; }
    public uint DefaultBattleNaviId { get; set; }
    public List<Navi> UserNavis { get; set; } = new();
}