using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Handlers.UI.Navi;

public record UpdateAllNaviCostumeCommand(UpdateAllNaviCostumeRequest Request) : IRequest<BasicResponse>;

public class UpdateAllNaviCostumeCommandHandler : IRequestHandler<UpdateAllNaviCostumeCommand, BasicResponse>
{
    private readonly ServerDbContext _context;

    public UpdateAllNaviCostumeCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpdateAllNaviCostumeCommand request, CancellationToken cancellationToken)
    {
        var updateAllNaviCostumeRequest = request.Request;
        
        var cardProfile = _context.CardProfiles
            .Include(x => x.Navis)
            .FirstOrDefault(x =>
                x.AccessCode == updateAllNaviCostumeRequest.AccessCode && x.ChipId == updateAllNaviCostumeRequest.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var currentGuestNavs = cardProfile.Navis;
        
        updateAllNaviCostumeRequest.Navis
            .ForEach(UpsertNavi(currentGuestNavs));
        
        _context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }

    Action<WebUIOver.Shared.Dto.Common.Navi> UpsertNavi(ICollection<Models.Cards.Navi.Navi> currentGuestNavs)
    {
        return navi =>
        {
            var guestNavi = currentGuestNavs.FirstOrDefault(guestNaviGroup => guestNaviGroup.GuestNavId == navi.Id);

            if (guestNavi is null)
            {
                var newNavi = new Models.Cards.Navi.Navi()
                {
                    GuestNavId = navi.Id,
                    GuestNavSettingFlag = false,
                    BattleNavSettingFlag = false,
                    NewCostumeFlag = false,
                    GuestNavCostume = navi.CostumeId
                };

                currentGuestNavs.Add(newNavi);
                return;
            }

            guestNavi.GuestNavCostume = navi.CostumeId;
        };
    }
}