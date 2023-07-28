using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Dto.Response;
using Server.Models.Cards;
using Server.Persistence;

namespace Server.Handlers.Card;

public record GetAllBareboneCardCommand() : IRequest<List<BareboneCardProfile>>;

public class GetAllBareboneCardCommandHandler : IRequestHandler<GetAllBareboneCardCommand, List<BareboneCardProfile>>
{
    private readonly ServerDbContext _context;

    public GetAllBareboneCardCommandHandler(ServerDbContext context)
    {
        _context = context;
    }
    
    public Task<List<BareboneCardProfile>> Handle(GetAllBareboneCardCommand request, CancellationToken cancellationToken)
    {
        List<CardProfile> cardProfiles = _context.CardProfiles
            .Include(x => x.UserDomain)
            .ToList();

        List<BareboneCardProfile> bareboneCardProfiles = cardProfiles
            .Select(ToBareboneCardProfile())
            .ToList();
        
        return Task.FromResult(bareboneCardProfiles);
    }

    private Func<CardProfile, BareboneCardProfile> ToBareboneCardProfile()
    {
        return cardProfile =>
        {
            var user = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
                
            return new BareboneCardProfile
            {
                accessCode = cardProfile.AccessCode,
                chipId = cardProfile.ChipId,
                userName = user.PlayerName
            };
        };
    }
}