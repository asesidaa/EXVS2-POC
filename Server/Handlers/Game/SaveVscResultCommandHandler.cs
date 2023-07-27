using MediatR;
using Newtonsoft.Json;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record SaveVscResultCommand(Request Request) : IRequest<Response>;

public class SaveVscResultCommandHandler : IRequestHandler<SaveVscResultCommand, Response>
{
    public Task<Response> Handle(SaveVscResultCommand request, CancellationToken cancellationToken)
    {
        var readPreLoadCardStr = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "preloadcard.json"));
        var preLoadCard = JsonConvert.DeserializeObject<Response.PreLoadCard>(readPreLoadCardStr);
        
        var readLoadCardStr = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "loadcard.json"));
        var loadCard = JsonConvert.DeserializeObject<Response.LoadCard>(readLoadCardStr);
        
        if (preLoadCard == null || loadCard == null)
        {
            return Task.FromResult(new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.Success,
                save_vsc_result = new Response.SaveVscResult()
            });
        }
        
        var resultFromRequest = request.Request.save_vsc_result.Result;
        var favouriteMsList = preLoadCard.User.FavoriteMobileSuits;
        var pilotDataGroup = loadCard.pilot_data_group;
        
        preLoadCard.load_player.EchelonId = resultFromRequest.EchelonId;
        preLoadCard.load_player.EchelonExp += resultFromRequest.EchelonExp;
        preLoadCard.load_player.SEchelonFlag = resultFromRequest.SEchelonFlag;
        preLoadCard.User.Gp += resultFromRequest.Gp;
        
        UpdateNaviFamiliarity(resultFromRequest, preLoadCard);
        
        UpsertMsUsedNum(resultFromRequest, pilotDataGroup, favouriteMsList);

        if (resultFromRequest.Partner.CpuFlag == 1)
        {
            UpsertForTriadBuddy(pilotDataGroup, resultFromRequest);
        }
        
        UpsertTriadSceneData(pilotDataGroup, resultFromRequest);

        String outPreLoadCardJsonStr = JsonConvert.SerializeObject(preLoadCard);
        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "preloadcard.json"), outPreLoadCardJsonStr);
            
        String outLoadCardJsonStr = JsonConvert.SerializeObject(loadCard);
        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "loadcard.json"),outLoadCardJsonStr);

        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsc_result = new Response.SaveVscResult()
        });
    }
    
    void UpdateNaviFamiliarity(Request.SaveVscResult.PlayResultGroup resultFromRequest, Response.PreLoadCard preLoadCard)
    {
        var uiNaviId = resultFromRequest.GuestNavId;
        var uiNavi = preLoadCard.User.GuestNavs
            .FirstOrDefault(guestNavi => guestNavi.GuestNavId == uiNaviId);

        if (uiNavi != null)
        {
            uiNavi.GuestNavFamiliarity += 1;
        }

        var battleNaviId = resultFromRequest.BattleNavId;
        var battleNavi = preLoadCard.User.GuestNavs
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

        if (msSkillGroup == null)
        {
            pilotDataGroup.MsSkills.Add(new Response.LoadCard.PilotDataGroup.MSSkillGroup
            {
                MstMobileSuitId = msId,
                MsUsedNum = 1,
                CostumeId = 0,
                TriadBuddyPoint = 0
            });
        }
        else
        {
            msSkillGroup.MsUsedNum++;
        }

        var favouriteMs = favouriteMsList
            .FirstOrDefault(favouriteMs => favouriteMs.MstMobileSuitId == msId);

        if (favouriteMs != null)
        {
            favouriteMs.MsUsedNum++;
        }
    }

    void UpsertForTriadBuddy(Response.LoadCard.PilotDataGroup pilotDataGroup, Request.SaveVscResult.PlayResultGroup resultFromRequest)
    {
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
            pilotDataGroup.CpuRibbons = pilotDataGroup.CpuRibbons
                .Concat(resultFromRequest.ReleasedRibbonIds)
                .Distinct()
                .ToArray();
        }
    }
}
