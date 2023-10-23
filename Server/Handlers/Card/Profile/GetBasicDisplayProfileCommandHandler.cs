using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Mappers;
using Server.Persistence;
using WebUI.Shared.Dto.Enum;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card.Profile;

public record GetBasicDisplayProfileCommand(string AccessCode, string ChipId) : IRequest<BasicDisplayProfile>;

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

        if (cardProfile.DistinctTeamFormationToken == string.Empty)
        {
            cardProfile.DistinctTeamFormationToken = Guid.NewGuid().ToString("n").Substring(0, 16);
            context.SaveChanges();
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
            UserId = (uint) cardProfile.Id,
            UserName = preLoadUser.PlayerName,
            OpenEchelon = preLoadUser.OpenEchelon,
            OpenRecord = preLoadUser.OpenRecord,
            DefaultGaugeDesignId = mobileUserGroup.Customize.DefaultGaugeDesignId,
            DefaultBgmPlayingMethod = (BgmPlayingMethod) mobileUserGroup.Customize.DefaultBgmPlayMethod,
            DefaultBgmList = mobileUserGroup.Customize.DefaultBgmSettings,
            DefaultTitle = preLoadUser.customize_group.DefaultTitleCustomize.ToTitle(),
            TriadTitle = mobileUserGroup.TriadTitleCustomize.ToTitle(),
            RankingTitle = mobileUserGroup.RankMatchTitleCustomize.ToTitle(),
            DistinctTeamFormationToken = cardProfile.DistinctTeamFormationToken
        };

        return Task.FromResult(basicDisplayProfile);
    }
}