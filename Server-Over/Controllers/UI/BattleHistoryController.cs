using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerOver.Handlers.UI.History;
using WebUIOver.Shared.Dto.History;

namespace ServerOver.Controllers.UI;

[ApiController]
[Route("ui/battle-history")]
public class BattleHistoryController(IMediator mediator) : BaseController<BattleHistoryController>
{
    [HttpGet("get-recent-battle-histories/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<List<BattleHistorySummary>>> GetRecentBattleHistories(String accessCode, String chipId)
    {
        var response = await mediator.Send(new GetRecentBattleHistoriesCommand(accessCode, chipId));
        return response;
    }
}