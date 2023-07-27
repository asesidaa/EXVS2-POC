using MediatR;
using Newtonsoft.Json;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record SaveVsmResultCommand(Request Request) : IRequest<Response>;

public class SaveVsmResultCommandHandler : IRequestHandler<SaveVsmResultCommand, Response>
{
    public Task<Response> Handle(SaveVsmResultCommand request, CancellationToken cancellationToken)
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
                save_vsm_on_result = new Response.SaveVsmOnResult()
            });
        }
        
        var resultFromRequest = request.Request.save_vsm_result.Result;
        var favouriteMsList = preLoadCard.User.FavoriteMobileSuits;
        var pilotDataGroup = loadCard.pilot_data_group;
        
        preLoadCard.load_player.EchelonId = resultFromRequest.EchelonId;
        preLoadCard.load_player.EchelonExp += resultFromRequest.EchelonExp;
        preLoadCard.load_player.SEchelonFlag = resultFromRequest.SEchelonFlag;
        preLoadCard.User.Gp += resultFromRequest.Gp;

        UpdateWinLossCount(request, resultFromRequest, preLoadCard);

        UpdateNaviFamiliarity(resultFromRequest, preLoadCard);
        
        UpsertMsUsedNum(resultFromRequest, pilotDataGroup, favouriteMsList);
        
        String outPreLoadCardJsonStr = JsonConvert.SerializeObject(preLoadCard);
        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "preloadcard.json"), outPreLoadCardJsonStr);
            
        String outLoadCardJsonStr = JsonConvert.SerializeObject(loadCard);
        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "loadcard.json"),outLoadCardJsonStr);
        
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsm_on_result = new Response.SaveVsmOnResult()
        });
    }
    
    void UpdateWinLossCount(SaveVsmResultCommand request, Request.SaveVsmResult.PlayResultGroup resultFromRequest,
        Response.PreLoadCard preLoadCard)
    {
        if (resultFromRequest.WinFlag)
        {
            preLoadCard.load_player.TotalWin++;
            if (request.Request.save_vsm_result.ShuffleFlag)
            {
                preLoadCard.load_player.ShuffleWin++;
            }
            else
            {
                preLoadCard.load_player.TeamWin++;
            }
        }
        else
        {
            preLoadCard.load_player.TotalLose++;
            if (request.Request.save_vsm_result.ShuffleFlag)
            {
                preLoadCard.load_player.ShuffleLose++;
            }
            else
            {
                preLoadCard.load_player.TeamLose++;
            }
        }
    }

    void UpdateNaviFamiliarity(Request.SaveVsmResult.PlayResultGroup resultFromRequest, Response.PreLoadCard preLoadCard)
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
    
    void UpsertMsUsedNum(Request.SaveVsmResult.PlayResultGroup resultFromRequest, Response.LoadCard.PilotDataGroup pilotDataGroup,
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
}
