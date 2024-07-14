using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using Server.Context.Battle;

namespace Server.Mappers.Context;

[Mapper]
public static partial class VscResultMapper
{
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.Gp) },
        new[] { nameof(BattleResultContext.CommonDomain), nameof(BattleResultContext.CommonDomain.GpIncrement) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.WinFlag) },
        new[] { nameof(BattleResultContext.CommonDomain), nameof(BattleResultContext.CommonDomain.IsWin) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.EchelonExp) },
        new[] { nameof(BattleResultContext.EchelonDomain), nameof(BattleResultContext.EchelonDomain.EchelonExp) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.SEchelonFlag) },
        new[] { nameof(BattleResultContext.EchelonDomain), nameof(BattleResultContext.EchelonDomain.SEchelonFlag) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.SEchelonProgress) },
        new[] { nameof(BattleResultContext.EchelonDomain), nameof(BattleResultContext.EchelonDomain.SEchelonProgress) }
    )]
    public static partial BattleResultContext ToBattleResultContext(this Request.SaveVscResult.PlayResultGroup resultGroup);
}