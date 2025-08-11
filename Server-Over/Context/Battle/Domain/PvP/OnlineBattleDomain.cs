using nue.protocol.exvs;

namespace ServerOver.Context.Battle.Domain.PvP;

public class OnlineBattleDomain
{
    public OnlineMatchingMode OnlineMatchingMode { get; set; } = new();
    public bool IsShuffle { get; set; } = false;
    public int LicenseScoreChange { get; set; } = 0;
}