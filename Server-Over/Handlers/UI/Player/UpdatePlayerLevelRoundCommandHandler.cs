using MediatR;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Handlers.UI.Player;

public record UpdatePlayerLevelRoundCommand(BasicCardRequest Request) : IRequest<BasicResponse>;

public class UpdatePlayerLevelRoundCommandHandler : IRequestHandler<UpdatePlayerLevelRoundCommand, BasicResponse>
{
    private readonly ServerDbContext _context;

    public UpdatePlayerLevelRoundCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpdatePlayerLevelRoundCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;
        
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var playerLevel = _context.PlayerLevelDbSet
            .First(x => x.CardProfile == cardProfile);

        if (playerLevel.PrestigeId >= 3)
        {
            return Task.FromResult(new BasicResponse
            {
                Success = true
            });
        }

        if (playerLevel.PlayerLevelId != 200)
        {
            return Task.FromResult(new BasicResponse
            {
                Success = true
            });
        }

        playerLevel.PlayerLevelId = 1;
        playerLevel.PlayerExp = 0;
        playerLevel.LevelMaxDispFlag = false;
        playerLevel.PrestigeId += 1;
        
        _context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
}