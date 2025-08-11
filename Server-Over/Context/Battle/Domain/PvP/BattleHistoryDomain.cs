using nue.protocol.exvs;

namespace ServerOver.Context.Battle.Domain.PvP;

public class BattleHistoryDomain
{
    public List<WarSituationGroup> PlayerActions { get; set; } = new();
    public AdversaryManGroup? Ally { get; set; } = null;
    public List<AdversaryManGroup> FilteredTargets { get; set; } = new();
}