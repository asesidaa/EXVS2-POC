using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Request;
using WebUIVanilla.Shared.Dto.Response;

namespace ServerVanilla.Handlers.Card.Navi;

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
            Success = false
        };

        var cardProfile = _context.CardProfiles
            .Include(x => x.Navi)
            .FirstOrDefault(x =>
                x.AccessCode == upsertDefaultNaviRequest.AccessCode && x.ChipId == upsertDefaultNaviRequest.ChipId);

        if (cardProfile == null)
        {
            return Task.FromResult(response);
        }
        
        var navis = cardProfile.Navi;

        navis.ToList()
            .ForEach(navi =>
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
                    new Models.Cards.Navi.Navi()
                    {
                        GuestNavId = upsertDefaultNaviRequest.DefaultUiNaviId,
                        GuestNavSettingFlag = true,
                        GuestNavRemains = 99999,
                        BattleNavSettingFlag = false,
                        BattleNavRemains = 99999,
                        GuestNavCostume = 0,
                        GuestNavFamiliarity = 0,
                        NewCostumeFlag = false,
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
                    new Models.Cards.Navi.Navi()
                    {
                        GuestNavId = upsertDefaultNaviRequest.DefaultBattleNaviId,
                        GuestNavSettingFlag = false,
                        GuestNavRemains = 99999,
                        BattleNavSettingFlag = true,
                        BattleNavRemains = 99999,
                        GuestNavCostume = 0,
                        GuestNavFamiliarity = 0,
                        NewCostumeFlag = false,
                    }
                );
            }
        }
        else
        {
            battleNavi.BattleNavSettingFlag = true;
        }

        _context.SaveChanges();

        response.Success = true;

        return Task.FromResult(response);
    }
}