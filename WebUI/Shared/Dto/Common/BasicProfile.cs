using WebUI.Shared.Dto.Enum;

namespace WebUI.Shared.Dto.Common;

public class BasicProfile
{
    public String userName { get; set; } = "";
    public BurstType defaultBurstType { get; set; } = BurstType.COVERING;
    public uint openRecord { get; set; } = 0;
    public uint openEchelon { get; set; } = 0;
    public uint defaultGaugeDesignId { get; set; } = 0;
    public BgmPlayingMethod defaultBgmPlayingMethod { get; set; } = BgmPlayingMethod.RANDOM;
    public uint[] defaultBgmList { get; set; } = Array.Empty<uint>();
}