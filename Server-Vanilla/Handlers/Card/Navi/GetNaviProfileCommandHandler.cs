using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Response;

namespace ServerVanilla.Handlers.Card.Navi;

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
            .Include(x => x.Navi)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var navis = cardProfile.Navi;

        var userNavis = navis
            .Select(navi => new WebUIVanilla.Shared.Dto.Common.Navi
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
            UserNavis = userNavis
        };

        return Task.FromResult(naviProfile);
    }
}