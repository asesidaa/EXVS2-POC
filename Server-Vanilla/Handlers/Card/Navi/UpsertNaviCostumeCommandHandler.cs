using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Request;
using WebUIVanilla.Shared.Dto.Response;

namespace ServerVanilla.Handlers.Card.Navi;

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
            Success = false
        };

        if (upsertNaviCostumeRequest.NaviId == 0)
        {
            return Task.FromResult(response);
        }

        var cardProfile = _context.CardProfiles
            .Include(x => x.Navi)
            .FirstOrDefault(x =>
                x.AccessCode == upsertNaviCostumeRequest.AccessCode && x.ChipId == upsertNaviCostumeRequest.ChipId);

        if (cardProfile == null)
        {
            return Task.FromResult(response);
        }
        
        var navis = cardProfile.Navi;
        
        var targetCostumeNavi = navis.FirstOrDefault(navi => navi.GuestNavId == upsertNaviCostumeRequest.NaviId);

        if (targetCostumeNavi == null)
        {
            navis.Add(
                new Models.Cards.Navi.Navi
                {
                    GuestNavId = upsertNaviCostumeRequest.NaviId,
                    GuestNavSettingFlag = false,
                    GuestNavRemains = 9999,
                    BattleNavSettingFlag = false,
                    BattleNavRemains = 9999,
                    GuestNavCostume = upsertNaviCostumeRequest.CostumeId,
                    GuestNavFamiliarity = 0,
                    NewCostumeFlag = false,
                }
            );
        }
        else
        {
            targetCostumeNavi.GuestNavCostume = upsertNaviCostumeRequest.CostumeId;
        }
        
        _context.SaveChanges();

        response.Success = true;

        return Task.FromResult(response);
    }
}