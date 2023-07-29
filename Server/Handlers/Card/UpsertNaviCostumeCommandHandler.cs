using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Dto.Request;
using Server.Dto.Response;
using Server.Persistence;

namespace Server.Handlers.Card;

public record UpsertNaviCostumeCommand(UpsertNaviCostumeRequest Request) : IRequest<BasicResponse>;

public class UpsertNaviCostumeCommandHandler : IRequestHandler<UpsertNaviCostumeCommand, BasicResponse>
{
    private readonly ServerDbContext _context;

    public UpsertNaviCostumeCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpsertNaviCostumeCommand request, CancellationToken cancellationToken)
    {
        var upsertNaviCostumeRequest = request.Request;
        
        var response = new BasicResponse
        {
            success = false
        };

        if (upsertNaviCostumeRequest.naviId == 0)
        {
            return Task.FromResult(response);
        }

        var cardProfile = _context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x =>
                x.AccessCode == upsertNaviCostumeRequest.accessCode && x.ChipId == upsertNaviCostumeRequest.chipId);

        if (cardProfile == null)
        {
            return Task.FromResult(response);
        }
        
        var user = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
        
        var navis = user.GuestNavs;
        
        var targetCostumeNavi = navis.FirstOrDefault(navi => navi.GuestNavId == upsertNaviCostumeRequest.naviId);

        if (targetCostumeNavi == null)
        {
            navis.Add(
                new Response.PreLoadCard.MobileUserGroup.GuestNavGroup
                {
                    GuestNavSettingFlag = false,
                    GuestNavId = upsertNaviCostumeRequest.naviId,
                    GuestNavCostume = upsertNaviCostumeRequest.costumeId,
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
            targetCostumeNavi.GuestNavCostume = upsertNaviCostumeRequest.costumeId;
        }
        
        cardProfile.UserDomain.UserJson = JsonConvert.SerializeObject(user);

        _context.SaveChanges();

        response.success = true;

        return Task.FromResult(response);
    }
}