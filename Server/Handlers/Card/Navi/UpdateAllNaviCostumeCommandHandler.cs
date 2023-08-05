using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Mappers;
using Server.Persistence;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;
using WebUI.Shared.Exception;

namespace Server.Handlers.Card.Navi;

public record UpdateAllNaviCostumeCommand(UpdateAllNaviCostumeRequest Request) : IRequest<BasicResponse>;

public class UpdateAllNaviCostumeCommandHandler : IRequestHandler<UpdateAllNaviCostumeCommand, BasicResponse>
{
    private readonly ServerDbContext context;

    public UpdateAllNaviCostumeCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<BasicResponse> Handle(UpdateAllNaviCostumeCommand request, CancellationToken cancellationToken)
    {
        var updateAllNaviCostumeRequest = request.Request;
        
        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x =>
                x.AccessCode == updateAllNaviCostumeRequest.AccessCode && x.ChipId == updateAllNaviCostumeRequest.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var user = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
        
        if (user is null)
        {
            throw new InvalidCardDataException("User is invalid");
        }

        var currentGuestNavs = user.GuestNavs;
        
        updateAllNaviCostumeRequest.Navis
            .ForEach(UpsertNavi(currentGuestNavs));
        
        cardProfile.UserDomain.UserJson = JsonConvert.SerializeObject(user);

        context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }

    Action<WebUI.Shared.Dto.Common.Navi> UpsertNavi(List<Response.PreLoadCard.MobileUserGroup.GuestNavGroup> currentGuestNavs)
    {
        return navi =>
        {
            var guestNavi = currentGuestNavs.FirstOrDefault(guestNaviGroup => guestNaviGroup.GuestNavId == navi.Id);

            if (guestNavi is null)
            {
                var newNavi = navi.ToGuestNaviGroup();
                newNavi.GuestNavSettingFlag = false;
                newNavi.BattleNavSettingFlag = false;
                newNavi.GuestNavRemains = 99999;
                newNavi.BattleNavRemains = 99999;
                newNavi.GuestNavFamiliarity = 0;
                newNavi.NewCostumeFlag = false;
                currentGuestNavs.Add(newNavi);
                return;
            }

            guestNavi.GuestNavCostume = navi.CostumeId;
        };
    }
}