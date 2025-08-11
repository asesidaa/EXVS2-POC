using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerOver.Handlers.UI.Rank;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Rank;

namespace ServerOver.Controllers.UI.Rank;

[ApiController]
[Route("ui/rank/player-level-rank")]
public class PlayerLevelRankController(IMediator mediator, ServerDbContext context) : BaseController<PlayerLevelRankController>
{
    [HttpGet("getPlayerLevelRanks")]
    [Produces("application/json")]
    public async Task<PlayerLevelRankData> GetPlayerLevelRanks()
    {
        var response = await mediator.Send(new GetPlayerLevelRankCommand());
        return response;
    }
}