using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using ServerOver.Context.Battle;

namespace ServerOver.Mapper.Context;

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
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.LevelId) },
        new[] { nameof(BattleResultContext.PlayerLevelDomain), nameof(BattleResultContext.PlayerLevelDomain.LevelIdBefore) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.LevelId) },
        new[] { nameof(BattleResultContext.PlayerLevelDomain), nameof(BattleResultContext.PlayerLevelDomain.LevelIdBefore) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.PlayerLevelId) },
        new[] { nameof(BattleResultContext.PlayerLevelDomain), nameof(BattleResultContext.PlayerLevelDomain.LevelIdAfter) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.LevelExp) },
        new[] { nameof(BattleResultContext.PlayerLevelDomain), nameof(BattleResultContext.PlayerLevelDomain.ExpIncrement) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.PrestigeId) },
        new[] { nameof(BattleResultContext.PlayerLevelDomain), nameof(BattleResultContext.PlayerLevelDomain.PrestigeId) }
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
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.PlayedAt) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.PlayedAt) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.VsElapsedTime) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.ElapsedSeconds) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.StageId) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.StageId) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.ResultScore) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.Score) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.ResultOrder) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.ScoreRank) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.GivenDamage) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.TotalGivenDamage) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.TakenDamage) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.TotalTakenDamage) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.ConsecutiveWin) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.ConsecutiveWinCount) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.NoDamageFlag) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.NoDamageFlag) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.BurstGivenDamage) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.TotalExBurstDamage) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.BurstType) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.BurstType) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.Burst) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.BurstCount) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.Overheat) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.OverheatCount) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.ComboGivenDamage) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.ComboGivenDamage) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVsmResult.PlayResultGroup.SkinId) },
        new[] { nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.SkinId) }
    )]
    public static partial BattleResultContext ToBattleResultContext(this Request.SaveVsmResult.PlayResultGroup resultGroup);
}