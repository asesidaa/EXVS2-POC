using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card.Navi;

public record UpsertNaviCostumeCommand(UpsertNaviCostumeRequest Request) : IRequest<BasicResponse>;

public class UpsertNaviCostumeCommandHandler : IRequestHandler<UpsertNaviCostumeCommand, BasicResponse>
{
    private readonly ServerDbContext context;

    public UpsertNaviCostumeCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<BasicResponse> Handle(UpsertNaviCostumeCommand request, CancellationToken cancellationToken)
    {
        var upsertNaviCostumeRequest = request.Request;
        
        var response = new BasicResponse
        {
            Success = false
        };

        if (upsertNaviCostumeRequest.NaviId == 0)
        {
            return Task.FromResult(response);
        }

        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x =>
                x.AccessCode == upsertNaviCostumeRequest.AccessCode && x.ChipId == upsertNaviCostumeRequest.ChipId);

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
        
        var targetCostumeNavi = navis.FirstOrDefault(navi => navi.GuestNavId == upsertNaviCostumeRequest.NaviId);

        if (targetCostumeNavi == null)
        {
            navis.Add(
                new Response.PreLoadCard.MobileUserGroup.GuestNavGroup
                {
                    GuestNavSettingFlag = false,
                    GuestNavId = upsertNaviCostumeRequest.NaviId,
                    GuestNavCostume = upsertNaviCostumeRequest.CostumeId,
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
            targetCostumeNavi.GuestNavCostume = upsertNaviCostumeRequest.CostumeId;
        }
        
        cardProfile.UserDomain.UserJson = JsonConvert.SerializeObject(user);

        context.SaveChanges();

        response.Success = true;

        return Task.FromResult(response);
    }
}