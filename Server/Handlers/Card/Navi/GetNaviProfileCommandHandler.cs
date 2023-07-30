using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card.Navi;

public record GetNaviProfileCommand(String AccessCode, String ChipId) : IRequest<NaviProfile>;

public class GetNaviProfileCommandHandler : IRequestHandler<GetNaviProfileCommand, NaviProfile>
{
    private readonly ServerDbContext context;

    public GetNaviProfileCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }
    
    public Task<NaviProfile> Handle(GetNaviProfileCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var user = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);

        if (user is null)
        {
            throw new NullReferenceException("User is invalid");
        }
        
        var navis = user.GuestNavs;

        var userNavis = navis
            .Select(navi => new WebUI.Shared.Dto.Common.Navi
            {
                Id = navi.GuestNavId,
                CostumeId = navi.GuestNavCostume.GetValueOrDefault(0),
                Familiarity = navi.GuestNavFamiliarity
            })
            .OrderBy(navi => navi.Id)
            .ToList();

        var defaultUiNavi = navis.LastOrDefault(navi => navi.GuestNavSettingFlag);
        var battleUiNavi = navis.LastOrDefault(navi => navi.BattleNavSettingFlag);

        var defaultUiNaviId = defaultUiNavi?.GuestNavId   ?? 0;
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