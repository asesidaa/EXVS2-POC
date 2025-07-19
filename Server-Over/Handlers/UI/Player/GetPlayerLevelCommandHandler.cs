using MediatR;
using ServerOver.Constants;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Player;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.Player;

public record GetPlayerLevelCommand(string AccessCode, string ChipId) : IRequest<PlayerLevelProfile>;


public class GetPlayerLevelCommandHandler : IRequestHandler<GetPlayerLevelCommand, PlayerLevelProfile>
{
    private readonly ServerDbContext _context;

    public GetPlayerLevelCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<PlayerLevelProfile> Handle(GetPlayerLevelCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile == null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        var playerLevel = _context.PlayerLevelDbSet
            .First(x => x.CardProfile == cardProfile);

        var playerLevelProfile = new PlayerLevelProfile()
        {
            RoundLevel = playerLevel.PrestigeId,
            PlayerLevel = playerLevel.PlayerLevelId,
            CurrentExp = playerLevel.PlayerExp,
            AbleToStepUpRound = playerLevel.LevelMaxDispFlag,
            ExpRequirement = new ExpRequirement()
            {
                Round1Exp = PlayerLevelExp.Round1Exp,
                Round2Exp = PlayerLevelExp.Round2Exp,
                Round3Exp = PlayerLevelExp.Round3Exp,
                RoundExExp = PlayerLevelExp.RoundExExp
            }
        };

        return Task.FromResult(playerLevelProfile);
    }
}