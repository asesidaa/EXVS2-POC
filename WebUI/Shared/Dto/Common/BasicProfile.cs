using WebUI.Shared.Dto.Enum;

namespace WebUI.Shared.Dto.Common;

public class BasicProfile
{
    public string UserName { get; set; } = string.Empty;
    public uint OpenRecord { get; set; } = 0;
    public uint OpenEchelon { get; set; } = 0;
    public uint DefaultGaugeDesignId { get; set; } = 0;
    public BgmPlayingMethod DefaultBgmPlayingMethod { get; set; } = BgmPlayingMethod.Random;
    public uint[] DefaultBgmList { get; set; } = Array.Empty<uint>();
}