using WebUIOver.Shared.Dto.Common;

namespace WebUIOVer.Shared.Dto.Response;

public class NaviProfile
{
    public uint DefaultUiNaviId { get; set; } = 0;
    public uint DefaultBattleNaviId { get; set; } = 0;
    public bool BattleNavAdviseFlag { get; set; } = true;
    public bool BattleNavNotifyFlag { get; set; } = true;
    public List<Navi> UserNavis { get; set; } = new();
}