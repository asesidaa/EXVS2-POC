using ServerVanilla.Context.Battle.Domain;
using ServerVanilla.Context.Battle.Domain.PvP;
using ServerVanilla.Context.Battle.Domain.Triad;

namespace ServerVanilla.Context.Battle;

public class BattleResultContext
{
    public CommonDomain CommonDomain { get; set; } = new();
    public EchelonDomain EchelonDomain { get; set; } = new ();
    public NaviDomain NaviDomain { get; set; } = new ();
    public MobileSuitMasteryDomain MobileSuitMasteryDomain { get; set; } = new ();
    public TeamDomain TeamDomain { get; set; } = new ();
    
    // The following Domain is Triad Mode Only
    public PartnerDomain PartnerDomain { get; set; } = new();
    public TriadInfoDomain TriadInfoDomain { get; set; } = new();
    
    // The following Domain is PvP Mode only
    public BattleStatisticDomain BattleStatisticDomain { get; set; } = new();
}