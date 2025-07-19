using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Enum;

namespace WebUIOVer.Shared.Dto.Common;

public class FavouriteMs
{
    public uint MsId { get; set; } = 0; // If msId = 0, the slot will be indicated as unused
    public uint GaugeDesignId { get; set; } = 0;
    public BgmPlayingMethod BgmPlayingMethod { get; set; } = BgmPlayingMethod.None;
    public uint[] BgmList { get; set; } = Array.Empty<uint>();
    public uint BattleNaviId { get; set; } // If the ID doesn't contain in User Navi List, it will be ignored during battle
    public BurstType BurstType { get; set; } = BurstType.Covering;
    public Title DefaultTitle { get; set; } = new();
    public Title TriadTitle { get; set; } = new();
    public Title ClassMatchTitle { get; set; } = new();
}