using ServerOver.Context.Battle.Domain;
using ServerOver.Context.Battle.Domain.PvP;
using ServerOver.Context.Battle.Domain.Triad;

namespace ServerOver.Context.Battle;

public class BattleResultContext
{
    public CommonDomain CommonDomain { get; set; } = new();
    public PlayerLevelDomain PlayerLevelDomain { get; set; } = new ();
    public NaviDomain NaviDomain { get; set; } = new ();
    public MobileSuitMasteryDomain MobileSuitMasteryDomain { get; set; } = new ();
    public TeamDomain TeamDomain { get; set; } = new ();
    
    // The following Domain is Triad Mode Only
    public PartnerDomain PartnerDomain { get; set; } = new();
    public TriadInfoDomain TriadInfoDomain { get; set; } = new();
    
    // The following Domain is PvP Mode only
    public BattleStatisticDomain BattleStatisticDomain { get; set; } = new();
    public OnlineBattleDomain OnlineBattleDomain { get; set; } = new();
    public BattleHistoryDomain BattleHistoryDomain { get; set; } = new();
}