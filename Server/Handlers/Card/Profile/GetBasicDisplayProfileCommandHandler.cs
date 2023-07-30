using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Enum;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card.Profile;

public record GetBasicDisplayProfileCommand(String AccessCode, String ChipId) : IRequest<BasicDisplayProfile>;

public class GetBasicDisplayProfileCommandHandler : IRequestHandler<GetBasicDisplayProfileCommand, BasicDisplayProfile>
{
    private readonly ServerDbContext context;

    public GetBasicDisplayProfileCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }
    
    public Task<BasicDisplayProfile> Handle(GetBasicDisplayProfileCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .Include(x => x.PilotDomain)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var preLoadUser = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
        if (preLoadUser is null)
        {
            throw new NullReferenceException("Preload user is invalid");
        }
        var mobileUserGroup =
            JsonConvert.DeserializeObject<Response.LoadCard.MobileUserGroup>(cardProfile.UserDomain
                .MobileUserGroupJson);
        if (mobileUserGroup is null)
        {
            throw new NullReferenceException("Mobile usergroup is invalid");
        }

        var basicDisplayProfile = new BasicDisplayProfile
        {
            UserName = preLoadUser.PlayerName,
            OpenEchelon = preLoadUser.OpenEchelon,
            OpenRecord = preLoadUser.OpenRecord,
            DefaultGaugeDesignId = mobileUserGroup.Customize.DefaultGaugeDesignId,
            DefaultBgmPlayingMethod = (BgmPlayingMethod) mobileUserGroup.Customize.DefaultBgmPlayMethod,
            DefaultBgmList = mobileUserGroup.Customize.DefaultBgmSettings,
            DefaultTitle = new Title
            {
                TextId = preLoadUser.customize_group.DefaultTitleCustomize.TitleTextId,
                OrnamentId = preLoadUser.customize_group.DefaultTitleCustomize.TitleOrnamentId,
                EffectId = preLoadUser.customize_group.DefaultTitleCustomize.TitleEffectId,
                BackgroundPartsId = preLoadUser.customize_group.DefaultTitleCustomize.TitleBackgroundPartsId
            },
            TriadTitle = new Title
            {
                TextId = mobileUserGroup.TriadTitleCustomize.TitleTextId,
                OrnamentId = mobileUserGroup.TriadTitleCustomize.TitleOrnamentId,
                EffectId = mobileUserGroup.TriadTitleCustomize.TitleEffectId,
                BackgroundPartsId = mobileUserGroup.TriadTitleCustomize.TitleBackgroundPartsId
            },
            RankingTitle = new Title
            {
                TextId = mobileUserGroup.RankMatchTitleCustomize.TitleTextId,
                OrnamentId = mobileUserGroup.RankMatchTitleCustomize.TitleOrnamentId,
                EffectId = mobileUserGroup.RankMatchTitleCustomize.TitleEffectId,
                BackgroundPartsId = mobileUserGroup.RankMatchTitleCustomize.TitleBackgroundPartsId
            }
        };

        return Task.FromResult(basicDisplayProfile);
    }
}