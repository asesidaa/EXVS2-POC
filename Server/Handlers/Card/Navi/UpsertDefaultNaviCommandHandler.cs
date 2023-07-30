using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card.Navi;

public record UpsertDefaultNaviCommand(UpsertDefaultNaviRequest Request) : IRequest<BasicResponse>;

public class UpsertDefaultNaviCommandHandler : IRequestHandler<UpsertDefaultNaviCommand, BasicResponse>
{
    private readonly ServerDbContext context;

    public UpsertDefaultNaviCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<BasicResponse> Handle(UpsertDefaultNaviCommand request, CancellationToken cancellationToken)
    {
        var upsertDefaultNaviRequest = request.Request;
        
        var response = new BasicResponse
        {
            Success = false
        };

        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x =>
                x.AccessCode == upsertDefaultNaviRequest.AccessCode && x.ChipId == upsertDefaultNaviRequest.ChipId);

        if (cardProfile == null)
        {
            return Task.FromResult(response);
        }
        
        var user = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
        if (user is null)
        {
            throw new NullReferenceException("User is invalid");
        }
        
        var navis = user.GuestNavs;
        
        navis.ForEach(navi =>
            {
                navi.GuestNavSettingFlag = false;
                navi.BattleNavSettingFlag = false;
            });

        var uiNavi = navis.FirstOrDefault(navi => navi.GuestNavId == upsertDefaultNaviRequest.DefaultUiNaviId);

        if (uiNavi == null )
        {
            if (upsertDefaultNaviRequest.DefaultUiNaviId > 0)
            {
                navis.Add(
                    new Response.PreLoadCard.MobileUserGroup.GuestNavGroup
                    {
                        GuestNavSettingFlag = true,
                        GuestNavId = upsertDefaultNaviRequest.DefaultUiNaviId,
                        GuestNavCostume = 0,
                        GuestNavFamiliarity = 0,
                        GuestNavRemains = 99999,
                        NewCostumeFlag = false,
                        BattleNavSettingFlag = false,
                        BattleNavRemains = 99999
                    }
                );
            }
        }
        else
        {
            uiNavi.GuestNavSettingFlag = true;
        }
        
        var battleNavi = navis.FirstOrDefault(navi => navi.GuestNavId == upsertDefaultNaviRequest.DefaultBattleNaviId);
        
        if (battleNavi == null)
        {
            if (upsertDefaultNaviRequest.DefaultBattleNaviId > 0)
            {
                navis.Add(
                    new Response.PreLoadCard.MobileUserGroup.GuestNavGroup
                    {
                        GuestNavSettingFlag = false,
                        GuestNavId = upsertDefaultNaviRequest.DefaultBattleNaviId,
                        GuestNavCostume = 0,
                        GuestNavFamiliarity = 0,
                        GuestNavRemains = 99999,
                        NewCostumeFlag = false,
                        BattleNavSettingFlag = true,
                        BattleNavRemains = 99999
                    }
                );
            }
        }
        else
        {
            battleNavi.BattleNavSettingFlag = true;
        }
        
        cardProfile.UserDomain.UserJson = JsonConvert.SerializeObject(user);

        context.SaveChanges();

        response.Success = true;

        return Task.FromResult(response);
    }
}