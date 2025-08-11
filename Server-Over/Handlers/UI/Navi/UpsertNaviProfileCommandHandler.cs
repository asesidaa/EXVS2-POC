using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Handlers.UI.Navi;

public record UpsertNaviProfileCommand(UpsertNaviProfileRequest Request) : IRequest<BasicResponse>;

public class UpsertNaviProfileCommandHandler : IRequestHandler<UpsertNaviProfileCommand, BasicResponse>
{
    private readonly ServerDbContext _context;

    public UpsertNaviProfileCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpsertNaviProfileCommand request, CancellationToken cancellationToken)
    {
        var upsertRequest = request.Request;
        
        var response = new BasicResponse
        {
            Success = false
        };

        var cardProfile = _context.CardProfiles
            .Include(x => x.Navis)
            .FirstOrDefault(x => x.AccessCode == upsertRequest.AccessCode && x.ChipId == upsertRequest.ChipId);

        if (cardProfile == null)
        {
            return Task.FromResult(response);
        }

        var navis = cardProfile.Navis;
            
        navis.ToList()
            .ForEach(navi =>
            {
                navi.GuestNavSettingFlag = false;
                navi.BattleNavSettingFlag = false;
            });

        var uiNavi = navis.FirstOrDefault(navi => navi.GuestNavId == upsertRequest.DefaultUiNaviId);

        if (uiNavi == null )
        {
            if (upsertRequest.DefaultUiNaviId > 0)
            {
                navis.Add(
                    new Models.Cards.Navi.Navi()
                    {
                        GuestNavId = upsertRequest.DefaultUiNaviId,
                        GuestNavSettingFlag = true,
                        BattleNavSettingFlag = false,
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
        
        var battleNavi = navis.FirstOrDefault(navi => navi.GuestNavId == upsertRequest.DefaultBattleNaviId);
        
        if (battleNavi == null)
        {
            if (upsertRequest.DefaultBattleNaviId > 0)
            {
                navis.Add(
                    new Models.Cards.Navi.Navi()
                    {
                        GuestNavId = upsertRequest.DefaultBattleNaviId,
                        GuestNavSettingFlag = false,
                        BattleNavSettingFlag = true,
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

        var naviSetting = _context.NaviSettingDbSet
            .First(x => x.CardProfile == cardProfile);

        naviSetting.BattleNavAdviseFlag = upsertRequest.BattleNavAdviseFlag;
        naviSetting.BattleNavNotifyFlag = upsertRequest.BattleNavNotifyFlag;

        _context.SaveChanges();

        response.Success = true;

        return Task.FromResult(response);
    }
}