using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Mapper.Card.Navi;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Request;
using WebUIVanilla.Shared.Dto.Response;

namespace ServerVanilla.Handlers.Card.Navi;

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
            .Include(x => x.Navi)
            .FirstOrDefault(x =>
                x.AccessCode == updateAllNaviCostumeRequest.AccessCode && x.ChipId == updateAllNaviCostumeRequest.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var currentGuestNavs = cardProfile.Navi;
        
        updateAllNaviCostumeRequest.Navis
            .ForEach(UpsertNavi(currentGuestNavs));
        
        _context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }

    Action<WebUIVanilla.Shared.Dto.Common.Navi> UpsertNavi(ICollection<Models.Cards.Navi.Navi> currentGuestNavs)
    {
        return navi =>
        {
            var guestNavi = currentGuestNavs.FirstOrDefault(guestNaviGroup => guestNaviGroup.GuestNavId == navi.Id);

            if (guestNavi is null)
            {
                var newNavi = navi.ToNavi();
                newNavi.GuestNavSettingFlag = false;
                newNavi.BattleNavSettingFlag = false;
                newNavi.GuestNavRemains = 9999;
                newNavi.BattleNavRemains = 9999;
                newNavi.GuestNavFamiliarity = 0;
                newNavi.NewCostumeFlag = false;
                currentGuestNavs.Add(newNavi);
                return;
            }

            guestNavi.GuestNavCostume = navi.CostumeId;
        };
    }
}