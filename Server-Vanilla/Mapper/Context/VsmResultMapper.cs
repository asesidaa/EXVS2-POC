using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using ServerVanilla.Context.Battle;

namespace ServerVanilla.Mapper.Context;

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
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.GuestNavId) },
        new[] { nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.GuestNavId) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.GuestNavFamiliarity) },
        new[] { nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.GuestNavFamiliarityIncrement) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.BattleNavId) },
        new[] { nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.BattleNavId) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.BattleNavFamiliarity) },
        new[] { nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.BattleNavFamiliarityIncrement) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.SkillPointMobileSuitId) },
        new[] { nameof(BattleResultContext.MobileSuitMasteryDomain), nameof(BattleResultContext.MobileSuitMasteryDomain.MasteryMobileSuitId) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.MstMobileSuitId) },
        new[] { nameof(BattleResultContext.MobileSuitMasteryDomain), nameof(BattleResultContext.MobileSuitMasteryDomain.ActualMobileSuitId) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.TagTeamId) },
        new[] { nameof(BattleResultContext.TeamDomain), nameof(BattleResultContext.TeamDomain.TeamId) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.TagSkillPoint) },
        new[] { nameof(BattleResultContext.TeamDomain), nameof(BattleResultContext.TeamDomain.TeamExp) }
    )]
    public static partial BattleResultContext ToBattleResultContext(this Request.SaveVsmResult.PlayResultGroup resultGroup);
}