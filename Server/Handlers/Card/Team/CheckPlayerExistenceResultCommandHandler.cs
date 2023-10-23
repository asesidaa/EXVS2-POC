using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card.Team;

public record CheckPlayerExistenceResultCommand(uint PartnerId, String PartnerToken) : IRequest<PlayerExistenceResult>;

public class CheckPlayerExistenceResultCommandHandler : IRequestHandler<CheckPlayerExistenceResultCommand, PlayerExistenceResult>
{
    private readonly ServerDbContext _context;
    
    public CheckPlayerExistenceResultCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<PlayerExistenceResult> Handle(CheckPlayerExistenceResultCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .Include(x => x.UserDomain)
            .Where(x => x.DistinctTeamFormationToken != "" && x.IsNewCard == false)
            .FirstOrDefault(x => x.Id == request.PartnerId && x.DistinctTeamFormationToken == request.PartnerToken);
        
        if (cardProfile == null)
        {
            return Task.FromResult(new PlayerExistenceResult
            {
                Success = false,
                PlayerName = ""
            });
        }
        
        var partnerMobileUserGroup = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
        
        if (partnerMobileUserGroup == null)
        {
            return Task.FromResult(new PlayerExistenceResult
            {
                Success = false,
                PlayerName = "N/A"
            });
        }

        return Task.FromResult(new PlayerExistenceResult
        {
            Success = true,
            PlayerId = (uint) cardProfile.Id,
            PlayerName = partnerMobileUserGroup.PlayerName
        });
    }
}