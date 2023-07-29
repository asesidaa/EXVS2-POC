using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Enum;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card.Profile;

public record GetBasicDisplayProfileCommand(String accessCode, String chipId) : IRequest<BasicDisplayProfile>;

public class GetBasicDisplayProfileCommandHandler : IRequestHandler<GetBasicDisplayProfileCommand, BasicDisplayProfile>
{
    private readonly ServerDbContext _context;

    public GetBasicDisplayProfileCommandHandler(ServerDbContext context)
    {
        _context = context;
    }
    
    public Task<BasicDisplayProfile> Handle(GetBasicDisplayProfileCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .Include(x => x.UserDomain)
            .Include(x => x.PilotDomain)
            .FirstOrDefault(x => x.AccessCode == request.accessCode && x.ChipId == request.chipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var preLoadUser = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
        var mobileUserGroup =
            JsonConvert.DeserializeObject<Response.LoadCard.MobileUserGroup>(cardProfile.UserDomain
                .MobileUserGroupJson);

        var basicDisplayProfile = new BasicDisplayProfile
        {
            userName = preLoadUser.PlayerName,
            defaultBurstType = (BurstType) mobileUserGroup.BurstType,
            openEchelon = preLoadUser.OpenEchelon,
            openRecord = preLoadUser.OpenRecord,
            defaultGaugeDesignId = mobileUserGroup.Customize.DefaultGaugeDesignId,
            defaultBgmPlayingMethod = (BgmPlayingMethod) mobileUserGroup.Customize.DefaultBgmPlayMethod,
            defaultBgmList = mobileUserGroup.Customize.DefaultBgmSettings
        };

        return Task.FromResult(basicDisplayProfile);
    }
}