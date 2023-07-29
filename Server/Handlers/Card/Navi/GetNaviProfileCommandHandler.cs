using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card.Navi;

public record GetNaviProfileCommand(String accessCode, String chipId) : IRequest<NaviProfile>;

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
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.AccessCode == request.accessCode && x.ChipId == request.chipId);

        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var user = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
        
        var navis = user.GuestNavs;

        var userNavis = navis
            .Select(navi => new WebUI.Shared.Dto.Common.Navi
            {
                id = navi.GuestNavId,
                costumeId = navi.GuestNavCostume.GetValueOrDefault(0),
                familiarity = navi.GuestNavFamiliarity
            })
            .OrderBy(navi => navi.id)
            .ToList();

        uint defaultUiNaviId = 0;
        uint defaultBattleNaviId = 0;

        var defaultUiNavi = navis.LastOrDefault(navi => navi.GuestNavSettingFlag);
        var battleUiNavi = navis.LastOrDefault(navi => navi.BattleNavSettingFlag);

        if (defaultUiNavi != null)
        {
            defaultUiNaviId = defaultUiNavi.GuestNavId;
        }
        
        if (battleUiNavi != null)
        {
            defaultBattleNaviId = battleUiNavi.GuestNavId;
        }

        var naviProfile = new NaviProfile
        {
            defaultUiNaviId = defaultUiNaviId,
            defaultBattleNaviId = defaultBattleNaviId,
            userNavis = userNavis
        };
        
        return Task.FromResult(naviProfile);
    }
}