using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using Server.Context.Battle;

namespace Server.Mappers.Context;

[Mapper]
public static partial class VsmResultMapper
{
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.Gp) },
        new[] { nameof(BattleResultContext.CommonDomain), nameof(BattleResultContext.CommonDomain.GpIncrement) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.WinFlag) },
        new[] { nameof(BattleResultContext.CommonDomain), nameof(BattleResultContext.CommonDomain.IsWin) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.EchelonExp) },
        new[] { nameof(BattleResultContext.EchelonDomain), nameof(BattleResultContext.EchelonDomain.EchelonExp) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.SEchelonFlag) },
        new[] { nameof(BattleResultContext.EchelonDomain), nameof(BattleResultContext.EchelonDomain.SEchelonFlag) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.SEchelonProgress) },
        new[] { nameof(BattleResultContext.EchelonDomain), nameof(BattleResultContext.EchelonDomain.SEchelonProgress) }
    )]
    public static partial BattleResultContext ToBattleResultContext(this Request.SaveVsmResult.PlayResultGroup resultGroup);
}