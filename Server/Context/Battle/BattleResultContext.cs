using Server.Context.Battle.Domain;

namespace Server.Context.Battle;

public class BattleResultContext
{
    public CommonDomain CommonDomain { get; set; } = new();
    public EchelonDomain EchelonDomain { get; set; } = new ();
}