using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Dto.Request;
using Server.Dto.Response;
using Server.Persistence;

namespace Server.Handlers.Card;

public record UpsertDefaultNaviCommand(UpsertDefaultNaviRequest Request) : IRequest<BasicResponse>;

public class UpsertDefaultNaviCommandHandler : IRequestHandler<UpsertDefaultNaviCommand, BasicResponse>
{
    private readonly ServerDbContext _context;

    public UpsertDefaultNaviCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpsertDefaultNaviCommand request, CancellationToken cancellationToken)
    {
        var upsertDefaultNaviRequest = request.Request;
        
        var response = new BasicResponse
        {
            success = false
        };

        var cardProfile = _context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x =>
                x.AccessCode == upsertDefaultNaviRequest.accessCode && x.ChipId == upsertDefaultNaviRequest.chipId);

        if (cardProfile == null)
        {
            return Task.FromResult(response);
        }
        
        var user = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
        
        var navis = user.GuestNavs;
        
        navis.ForEach(navi =>
            {
                navi.GuestNavSettingFlag = false;
                navi.BattleNavSettingFlag = false;
            });

        var uiNavi = navis.FirstOrDefault(navi => navi.GuestNavId == upsertDefaultNaviRequest.defaultUiNaviId);

        if (uiNavi == null && upsertDefaultNaviRequest.defaultUiNaviId > 0)
        {
            navis.Add(
                new Response.PreLoadCard.MobileUserGroup.GuestNavGroup
                {
                    GuestNavSettingFlag = true,
                    GuestNavId = upsertDefaultNaviRequest.defaultUiNaviId,
                    GuestNavCostume = 0,
                    GuestNavFamiliarity = 0,
                    GuestNavRemains = 99999,
                    NewCostumeFlag = false,
                    BattleNavSettingFlag = false,
                    BattleNavRemains = 99999
                }
            );
        }
        else
        {
            uiNavi.GuestNavSettingFlag = true;
        }
        
        var battleNavi = navis.FirstOrDefault(navi => navi.GuestNavId == upsertDefaultNaviRequest.defaultBattleNaviId);
        
        if (battleNavi == null && upsertDefaultNaviRequest.defaultBattleNaviId > 0)
        {
            navis.Add(
                new Response.PreLoadCard.MobileUserGroup.GuestNavGroup
                {
                    GuestNavSettingFlag = false,
                    GuestNavId = upsertDefaultNaviRequest.defaultUiNaviId,
                    GuestNavCostume = 0,
                    GuestNavFamiliarity = 0,
                    GuestNavRemains = 99999,
                    NewCostumeFlag = false,
                    BattleNavSettingFlag = true,
                    BattleNavRemains = 99999
                }
            );
        }
        else
        {
            battleNavi.BattleNavSettingFlag = true;
        }
        
        cardProfile.UserDomain.UserJson = JsonConvert.SerializeObject(user);

        _context.SaveChanges();

        response.success = true;

        return Task.FromResult(response);
    }
}