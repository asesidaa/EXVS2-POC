using MediatR;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Handlers.UI.Team;

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
        
        return Task.FromResult(new PlayerExistenceResult
        {
            Success = true,
            PlayerId = (uint) cardProfile.Id,
            PlayerName = cardProfile.UserName
        });
    }
}