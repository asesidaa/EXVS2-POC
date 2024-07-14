using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Commands.SaveBattle;
using Server.Mappers.Context;
using Server.Models.Cards;
using Server.Persistence;
using WebUI.Shared.Dto.Enum;

namespace Server.Handlers.Game;

public record SaveVscResultCommand(Request Request) : IRequest<Response>;

public class SaveVscResultCommandHandler : IRequestHandler<SaveVscResultCommand, Response>
{
    private readonly ILogger<SaveVscResultCommandHandler> _logger;
    private readonly ServerDbContext _context;

    public SaveVscResultCommandHandler(ILogger<SaveVscResultCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public Task<Response> Handle(SaveVscResultCommand request, CancellationToken cancellationToken)
    {
        var sessionId = request.Request.save_vsc_result.SessionId;

        var cardProfile = _context.CardProfiles
            .Include(x => x.PilotDomain)
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.SessionId == sessionId);

        if (cardProfile == null)
        {
            return Task.FromResult(new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.ErrServer,
                save_vsc_result = new Response.SaveVscResult()
            });
        }
        
        var pilotId = request.Request.save_vsc_result.PilotId;
        
        var resultFromRequest = request.Request.save_vsc_result.Result;
        var loadPlayer = JsonConvert.DeserializeObject<Response.PreLoadCard.LoadPlayer>(cardProfile.PilotDomain.LoadPlayerJson);
        var user = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
        var pilotData = JsonConvert.DeserializeObject<Response.LoadCard.PilotDataGroup>(cardProfile.PilotDomain.PilotDataGroupJson);
        var mobileUser = JsonConvert.DeserializeObject<Response.LoadCard.MobileUserGroup>(cardProfile.UserDomain.MobileUserGroupJson);
        
        if (loadPlayer == null || user == null || pilotData == null || mobileUser == null)
        {
            return Task.FromResult(new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.ErrServer,
                save_vsc_result = new Response.SaveVscResult()
            });
        }
        
        var oldEchelonId = loadPlayer.EchelonId;

        var triadBattleResult = new TriadBattleResult()
        {
            CourseId = resultFromRequest.CourseId,
            SceneId = resultFromRequest.SceneId,
            Mode = "Triad",
            WinFlag = resultFromRequest.WinFlag,
            Score = resultFromRequest.SceneScore,
            UsedMsId = resultFromRequest.MstMobileSuitId,
            UsedBurstType = resultFromRequest.BurstType,
            ElapsedSecond = resultFromRequest.VsElapsedTime,
            PastEchelonId = oldEchelonId,
            EchelonExpChange = resultFromRequest.EchelonExp,
            EchelonIdAfterBattle = resultFromRequest.EchelonId,
            TotalEchelonExp = resultFromRequest.EchelonExp + loadPlayer.EchelonExp,
            FullBattleResultJson = JsonConvert.SerializeObject(resultFromRequest)
        };
        
        var battleResultContext = resultFromRequest.ToBattleResultContext();
        battleResultContext.CommonDomain.BattleMode = BattleModeConstant.Triad;
        
        var favouriteMsList = user.FavoriteMobileSuits;
        
        user.Gp += resultFromRequest.Gp;
        
        UpdateNaviFamiliarity(resultFromRequest, user);
        
        UpsertMsUsedNum(resultFromRequest, pilotData, favouriteMsList);

        if (resultFromRequest.Partner.CpuFlag == 1)
        {
            UpsertForTriadBuddy(pilotData, resultFromRequest);
        }
        
        UpsertTriadSceneData(pilotData, resultFromRequest);

        UpsertRankData(resultFromRequest, loadPlayer, pilotData);
        
        UpdateTagTeam(resultFromRequest, pilotId);
        
        var saveBattleDataCommands = new List<ISaveBattleCommand>()
        {
            new SaveEchelonCommand(_context)
        };
        
        saveBattleDataCommands.ForEach(command => command.Save(cardProfile, loadPlayer, user, pilotData, mobileUser, battleResultContext));

        triadBattleResult.EchelonIdAfterBattle = loadPlayer.EchelonId;
        
        cardProfile.TriadBattleResults.Add(triadBattleResult);
        
        cardProfile.PilotDomain.LoadPlayerJson = JsonConvert.SerializeObject(loadPlayer);
        cardProfile.PilotDomain.PilotDataGroupJson = JsonConvert.SerializeObject(pilotData);
        cardProfile.UserDomain.UserJson = JsonConvert.SerializeObject(user);
        cardProfile.UserDomain.MobileUserGroupJson = JsonConvert.SerializeObject(mobileUser);
        
        _context.SaveChanges();

        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsc_result = new Response.SaveVscResult()
        });
    }
    
    private void UpdateTagTeam(Request.SaveVscResult.PlayResultGroup resultFromRequest, uint pilotId)
    {
        if (resultFromRequest.TagTeamId == 0)
        {
            return;
        }

        var tagTeam = _context.TagTeamData
            .FirstOrDefault(dbTagTeam =>
                dbTagTeam.CardId == pilotId && dbTagTeam.Id == resultFromRequest.TagTeamId);

        if (tagTeam is null)
        {
            return;
        }
        
        _logger.LogInformation("Team ID = {teamId}, Skill Point Increment = {increment}", 
            resultFromRequest.TagTeamId,
            resultFromRequest.TagSkillPoint  
        );
        
        tagTeam.SkillPoint += resultFromRequest.TagSkillPoint;
    }

    private void UpsertRankData(Request.SaveVscResult.PlayResultGroup resultFromRequest, Response.PreLoadCard.LoadPlayer loadPlayer, Response.LoadCard.PilotDataGroup pilotData)
    {
        if (resultFromRequest.rank_match_info is null)
        {
            return;
        }

        loadPlayer.RankIdSolo = resultFromRequest.rank_match_info.RankIdSolo;
        loadPlayer.RankIdTeam = resultFromRequest.rank_match_info.RankIdTeam;

        if (pilotData.pilot_rank_match is null)
        {
            pilotData.pilot_rank_match = new Response.LoadCard.PilotDataGroup.PilotRankMatch
            {
                PilotRankMatchSolo = CreateNewPilotRankMatchInfo(resultFromRequest.rank_match_info.RankIdSolo),
                PilotRankMatchTeam = CreateNewPilotRankMatchInfo(resultFromRequest.rank_match_info.RankIdTeam)
            };
        }
        else
        {
            if (pilotData.pilot_rank_match.PilotRankMatchSolo is null)
            {
                pilotData.pilot_rank_match.PilotRankMatchSolo =
                    CreateNewPilotRankMatchInfo(resultFromRequest.rank_match_info.RankIdSolo);
            }
            else
            {
                pilotData.pilot_rank_match.PilotRankMatchSolo.RankId = resultFromRequest.rank_match_info.RankIdSolo;
            }

            if (pilotData.pilot_rank_match.PilotRankMatchTeam is null)
            {
                pilotData.pilot_rank_match.PilotRankMatchTeam =
                    CreateNewPilotRankMatchInfo(resultFromRequest.rank_match_info.RankIdTeam);
            }
            else
            {
                pilotData.pilot_rank_match.PilotRankMatchTeam.RankId = resultFromRequest.rank_match_info.RankIdTeam;
            }
        }
    }

    void UpdateNaviFamiliarity(Request.SaveVscResult.PlayResultGroup resultFromRequest, Response.PreLoadCard.MobileUserGroup mobileUserGroup)
    {
        var uiNaviId = resultFromRequest.GuestNavId;
        var uiNavi = mobileUserGroup.GuestNavs
            .FirstOrDefault(guestNavi => guestNavi.GuestNavId == uiNaviId);

        if (uiNavi != null)
        {
            uiNavi.GuestNavFamiliarity += 1;
        }

        var battleNaviId = resultFromRequest.BattleNavId;
        var battleNavi = mobileUserGroup.GuestNavs
            .FirstOrDefault(guestNavi => guestNavi.GuestNavId == battleNaviId);

        if (battleNavi != null)
        {
            battleNavi.GuestNavFamiliarity += 1;
        }
    }
    
    void UpsertMsUsedNum(Request.SaveVscResult.PlayResultGroup resultFromRequest, Response.LoadCard.PilotDataGroup pilotDataGroup,
        List<Response.PreLoadCard.MobileUserGroup.FavoriteMSGroup> favouriteMsList)
    {
        var msId = resultFromRequest.SkillPointMobileSuitId;

        var msSkillGroup = pilotDataGroup.MsSkills
            .FirstOrDefault(msSkillGroup => msSkillGroup.MstMobileSuitId == msId);
        uint msSkillGroupUsedNum = 1;

        if (msSkillGroup == null)
        {
            pilotDataGroup.MsSkills.Add(new Response.LoadCard.PilotDataGroup.MSSkillGroup
            {
                MstMobileSuitId = msId,
                MsUsedNum = msSkillGroupUsedNum,
                CostumeId = 0,
                TriadBuddyPoint = 0
            });
        }
        else
        {
            msSkillGroup.MsUsedNum++;
            msSkillGroupUsedNum = msSkillGroup.MsUsedNum;
        }

        favouriteMsList
            .FindAll(favouriteMs => favouriteMs.MstMobileSuitId == msId)
            .ForEach(favouriteMs => favouriteMs.MsUsedNum = msSkillGroupUsedNum);
    }

    void UpsertForTriadBuddy(Response.LoadCard.PilotDataGroup pilotDataGroup, Request.SaveVscResult.PlayResultGroup resultFromRequest)
    {
        if (resultFromRequest.Partner.MobileSetFlag == false)
        {
            return;
        }
        
        var msSkillGroup = pilotDataGroup.MsSkills
            .FirstOrDefault(msSkillGroup => msSkillGroup.MstMobileSuitId == resultFromRequest.Partner.MstMobileSuitId);

        if (msSkillGroup == null)
        {
            pilotDataGroup.MsSkills.Add(new Response.LoadCard.PilotDataGroup.MSSkillGroup
            {
                MstMobileSuitId = resultFromRequest.Partner.MstMobileSuitId,
                TriadBuddyPoint = 1,
                CostumeId = 0,
                MsUsedNum = 0
            });
        }
        else
        {
            msSkillGroup.TriadBuddyPoint += 1;
        }
    }

    void UpsertTriadSceneData(Response.LoadCard.PilotDataGroup pilotDataGroup, Request.SaveVscResult.PlayResultGroup resultFromRequest)
    {
        pilotDataGroup.TotalTriadScenePlayNum += 1;
        pilotDataGroup.TotalTriadScore += resultFromRequest.SceneScore;
        
        var cpuSceneData = pilotDataGroup.CpuScenes
            .FirstOrDefault(data => data.CourseId == resultFromRequest.CourseId);

        if (cpuSceneData == null)
        {
            pilotDataGroup.CpuScenes.Add(new Response.LoadCard.PilotDataGroup.CpuSceneData
            {
                CourseId = resultFromRequest.CourseId,
                ReleasedAt = (ulong) DateTimeOffset.Now.ToUnixTimeSeconds(),
                TotalPlayNum = resultFromRequest.CourseClearFlag.GetValueOrDefault(false) ? 1u : 0u,
                TotalClearNum = resultFromRequest.CourseClearFlag.GetValueOrDefault(false) ? 1u : 0u,
                Highscore = resultFromRequest.SceneScore
            });
        }
        else
        {
            var courseScore = resultFromRequest.CourseScore.GetValueOrDefault(0);
            if (courseScore >= cpuSceneData.Highscore)
            {
                cpuSceneData.Highscore = courseScore;
            }
            
            if (resultFromRequest.CourseClearFlag.GetValueOrDefault(false))
            {
                cpuSceneData.TotalPlayNum += 1;
                cpuSceneData.TotalClearNum += 1;
            }
        }

        if (resultFromRequest.ReleasedRibbonIds != null)
        {
            if (pilotDataGroup.CpuRibbons == null)
            {
                pilotDataGroup.CpuRibbons = resultFromRequest.ReleasedRibbonIds;
            }
            else
            {
                pilotDataGroup.CpuRibbons = pilotDataGroup.CpuRibbons
                    .Concat(resultFromRequest.ReleasedRibbonIds)
                    .Distinct()
                    .ToArray();
            }
        }

        UpsertForReleaseCourse(pilotDataGroup, resultFromRequest);
    }

    void UpsertForReleaseCourse(Response.LoadCard.PilotDataGroup pilotDataGroup, Request.SaveVscResult.PlayResultGroup resultFromRequest)
    {
        if (resultFromRequest.ReleasedCourseIds is null)
        {
            return;
        }
        
        var releaseCourseIdList = resultFromRequest.ReleasedCourseIds.ToList();
        releaseCourseIdList
            .ForEach(courseId =>
            {
                var existingCpuScene = pilotDataGroup.CpuScenes
                    .FirstOrDefault(data => data.CourseId == resultFromRequest.CourseId);

                if (existingCpuScene is null)
                {
                    pilotDataGroup.CpuScenes.Add(new Response.LoadCard.PilotDataGroup.CpuSceneData
                    {
                        CourseId = courseId,
                        ReleasedAt = (ulong) DateTimeOffset.Now.ToUnixTimeSeconds(),
                        TotalPlayNum = 0,
                        TotalClearNum = 0,
                        Highscore = 0
                    });
                }
            });
    }

    Response.LoadCard.PilotDataGroup.PilotRankMatch.PilotRankMatchInfo CreateNewPilotRankMatchInfo(uint rankId)
    {
        return new Response.LoadCard.PilotDataGroup.PilotRankMatch.PilotRankMatchInfo
        {
            RankId = rankId,
            Level = 0,
            WinLoseInfoes = new uint[] {},
            RankPoint = 0,
            ExRank = 0,
            ExRankChangeFlag = 0,
            CpuNum = 0,
            ExxLockFlag = false,
            PreTrialExxFlag = false
        };
    }
}
