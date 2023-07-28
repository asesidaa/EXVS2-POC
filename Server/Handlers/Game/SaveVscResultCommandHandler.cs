using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;

namespace Server.Handlers.Game;

public record SaveVscResultCommand(Request Request) : IRequest<Response>;

public class SaveVscResultCommandHandler : IRequestHandler<SaveVscResultCommand, Response>
{
    private readonly ServerDbContext _context;

    public SaveVscResultCommandHandler(ServerDbContext context)
    {
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
        
        var favouriteMsList = user.FavoriteMobileSuits;
        
        loadPlayer.EchelonId = resultFromRequest.EchelonId;
        loadPlayer.EchelonExp += resultFromRequest.EchelonExp;
        loadPlayer.SEchelonFlag = resultFromRequest.SEchelonFlag;
        user.Gp += resultFromRequest.Gp;
        
        UpdateNaviFamiliarity(resultFromRequest, user);
        
        UpsertMsUsedNum(resultFromRequest, pilotData, favouriteMsList);

        if (resultFromRequest.Partner.CpuFlag == 1)
        {
            UpsertForTriadBuddy(pilotData, resultFromRequest);
        }
        
        UpsertTriadSceneData(pilotData, resultFromRequest);

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
        var msId = resultFromRequest.MstMobileSuitId;

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

        var favouriteMs = favouriteMsList
            .FirstOrDefault(favouriteMs => favouriteMs.MstMobileSuitId == msId);

        if (favouriteMs != null)
        {
            favouriteMs.MsUsedNum = msSkillGroupUsedNum;
        }
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
                ReleasedAt = 0,
                TotalPlayNum = 0,
                TotalClearNum = 0,
                Highscore = 0
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
    }
}
