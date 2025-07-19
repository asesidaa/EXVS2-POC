using MediatR;
using ServerOver.Persistence;
using WebUIOVer.Shared.Dto.Response;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.Navi;

public record GetNaviProfileCommand(string AccessCode, string ChipId) : IRequest<NaviProfile>;

public class GetNaviProfileCommandHandler : IRequestHandler<GetNaviProfileCommand, NaviProfile>
{
    private readonly ServerDbContext _context;

    public GetNaviProfileCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<NaviProfile> Handle(GetNaviProfileCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile == null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        var navis = _context.NaviDbSet
            .Where(x => x.CardProfile == cardProfile)
            .ToList();

        var naviSetting = _context.NaviSettingDbSet
            .First(x => x.CardProfile == cardProfile);

        var userNavis = navis
            .Select(navi => new WebUIOver.Shared.Dto.Common.Navi
            {
                Id = navi.GuestNavId,
                CostumeId = navi.GuestNavCostume,
                Familiarity = navi.GuestNavFamiliarity
            })
            .OrderBy(navi => navi.Id)
            .ToList();

        var defaultUiNavi = navis.LastOrDefault(navi => navi.GuestNavSettingFlag);
        var battleUiNavi = navis.LastOrDefault(navi => navi.BattleNavSettingFlag);

        var defaultUiNaviId = defaultUiNavi?.GuestNavId ?? 0;
        var defaultBattleNaviId = battleUiNavi?.GuestNavId ?? 0;
        
        var naviProfile = new NaviProfile
        {
            DefaultUiNaviId = defaultUiNaviId,
            DefaultBattleNaviId = defaultBattleNaviId,
            UserNavis = userNavis,
            BattleNavAdviseFlag = naviSetting.BattleNavAdviseFlag,
            BattleNavNotifyFlag = naviSetting.BattleNavNotifyFlag
        };

        return Task.FromResult(naviProfile);
    }
}