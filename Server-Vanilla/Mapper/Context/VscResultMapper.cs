using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using ServerVanilla.Context.Battle;

namespace ServerVanilla.Mapper.Context;

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
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.GuestNavId) },
        new[] { nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.GuestNavId) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.GuestNavFamiliarity) },
        new[] { nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.GuestNavFamiliarityIncrement) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.BattleNavId) },
        new[] { nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.BattleNavId) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.BattleNavFamiliarity) },
        new[] { nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.BattleNavFamiliarityIncrement) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.SkillPointMobileSuitId) },
        new[] { nameof(BattleResultContext.MobileSuitMasteryDomain), nameof(BattleResultContext.MobileSuitMasteryDomain.MasteryMobileSuitId) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.MstMobileSuitId) },
        new[] { nameof(BattleResultContext.MobileSuitMasteryDomain), nameof(BattleResultContext.MobileSuitMasteryDomain.ActualMobileSuitId) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.Partner), nameof(Request.SaveVscResult.PlayResultGroup.Partner.MobileSetFlag) },
        new[] { nameof(BattleResultContext.PartnerDomain), nameof(BattleResultContext.PartnerDomain.IsTriadPartner) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.Partner), nameof(Request.SaveVscResult.PlayResultGroup.Partner.MstMobileSuitId) },
        new[] { nameof(BattleResultContext.PartnerDomain), nameof(BattleResultContext.PartnerDomain.TriadPartnerMsId) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.TagTeamId) },
        new[] { nameof(BattleResultContext.TeamDomain), nameof(BattleResultContext.TeamDomain.TeamId) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.TagSkillPoint) },
        new[] { nameof(BattleResultContext.TeamDomain), nameof(BattleResultContext.TeamDomain.TeamExp) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.CourseId) },
        new[] { nameof(BattleResultContext.TriadInfoDomain), nameof(BattleResultContext.TriadInfoDomain.CourseId) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.SceneSeq) },
        new[] { nameof(BattleResultContext.TriadInfoDomain), nameof(BattleResultContext.TriadInfoDomain.SceneSeq) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.SceneId) },
        new[] { nameof(BattleResultContext.TriadInfoDomain), nameof(BattleResultContext.TriadInfoDomain.SceneId) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.SceneType) },
        new[] { nameof(BattleResultContext.TriadInfoDomain), nameof(BattleResultContext.TriadInfoDomain.SceneType) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.CourseCategory) },
        new[] { nameof(BattleResultContext.TriadInfoDomain), nameof(BattleResultContext.TriadInfoDomain.CourseCategory) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.TotalWantedDefeatNum) },
        new[] { nameof(BattleResultContext.TriadInfoDomain), nameof(BattleResultContext.TriadInfoDomain.TotalWantedDefeatNum) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.SceneScore) },
        new[] { nameof(BattleResultContext.TriadInfoDomain), nameof(BattleResultContext.TriadInfoDomain.SceneScore) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.CourseScore) },
        new[] { nameof(BattleResultContext.TriadInfoDomain), nameof(BattleResultContext.TriadInfoDomain.CourseScore) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.CourseClearFlag) },
        new[] { nameof(BattleResultContext.TriadInfoDomain), nameof(BattleResultContext.TriadInfoDomain.CourseClearFlag) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.ReleasedRibbonIds) },
        new[] { nameof(BattleResultContext.TriadInfoDomain), nameof(BattleResultContext.TriadInfoDomain.ReleasedRibbonIds) }
    )]
    [MapProperty(
        new[] { nameof(Request.SaveVscResult.PlayResultGroup.ReleasedCourseIds) },
        new[] { nameof(BattleResultContext.TriadInfoDomain), nameof(BattleResultContext.TriadInfoDomain.ReleasedCourseIds) }
    )]
    public static partial BattleResultContext ToBattleResultContext(this Request.SaveVscResult.PlayResultGroup resultGroup);
}