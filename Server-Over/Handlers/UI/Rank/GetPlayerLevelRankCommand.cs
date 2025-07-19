using MediatR;
using ServerOver.Constants;
using ServerOver.Mapper.Rank;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Player;
using WebUIOver.Shared.Dto.Rank;

namespace ServerOver.Handlers.UI.Rank;

public record GetPlayerLevelRankCommand : IRequest<PlayerLevelRankData>;

public class GetPlayerLevelRankCommandHandler
    : IRequestHandler<GetPlayerLevelRankCommand, PlayerLevelRankData>
{
    private readonly ServerDbContext _context;

    public GetPlayerLevelRankCommandHandler(ServerDbContext context)
    {
        _context = context;
    }
    
    public Task<PlayerLevelRankData> Handle(GetPlayerLevelRankCommand request, CancellationToken cancellationToken)
    {
        var playerLevelRankDtos = _context.PlayerLevelRankViews
            .Where(view => view.Rank <= 100)
            .OrderBy(view => view.Rank)
            .Select(view => view.ToPlayerLevelRankDto())
            .ToList();

        var expRequirement = new ExpRequirement()
        {
            Round1Exp = PlayerLevelExp.Round1Exp,
            Round2Exp = PlayerLevelExp.Round2Exp,
            Round3Exp = PlayerLevelExp.Round3Exp,
            RoundExExp = PlayerLevelExp.RoundExExp
        };
        
        return Task.FromResult(new PlayerLevelRankData
        {
            PlayerLevelRanks = playerLevelRankDtos,
            ExpRequirement = expRequirement
        });
    }
}