using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Mapper.Card.Title;
using ServerVanilla.Models.Cards;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Enum;
using WebUIVanilla.Shared.Dto.Response;

namespace ServerVanilla.Handlers.Card.Profile;

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
        var cardProfile = Queryable
            .FirstOrDefault<CardProfile>(
                context.CardProfiles
                        .Include(x => x.PlayerProfile)
                        .Include(x => x.CustomizeProfile) 
                        .Include(x => x.DefaultTitle), 
            x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        if (cardProfile.DistinctTeamFormationToken == string.Empty)
        {
            cardProfile.DistinctTeamFormationToken = Guid.NewGuid().ToString("n").Substring(0, 16);
            context.SaveChanges();
        }
        
        var defaultBgmList = Array.Empty<uint>();

        if (cardProfile.CustomizeProfile.DefaultBgmSettings != string.Empty)
        {
            defaultBgmList = Array.ConvertAll(cardProfile.CustomizeProfile.DefaultBgmSettings.Split(','), Convert.ToUInt32);
        }
        
        var basicDisplayProfile = new BasicDisplayProfile
        {
            UserId = (uint) cardProfile.Id,
            UserName = cardProfile.UserName,
            OpenEchelon = cardProfile.PlayerProfile.OpenEchelon,
            OpenRecord = cardProfile.PlayerProfile.OpenRecord,
            DefaultGaugeDesignId = cardProfile.CustomizeProfile.DefaultGaugeDesignId,
            DefaultBgmPlayingMethod = (BgmPlayingMethod) cardProfile.CustomizeProfile.DefaultBgmPlayMethod,
            DefaultBgmList = defaultBgmList,
            DefaultTitle = cardProfile.DefaultTitle.ToTitle(),
            DistinctTeamFormationToken = cardProfile.DistinctTeamFormationToken
        };

        return Task.FromResult(basicDisplayProfile);
    }
}