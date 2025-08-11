using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerOver.Handlers.UI.Player;
using WebUIOver.Shared.Dto.Player;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Controllers.UI;

[ApiController]
[Route("ui/player-level")]
public class PlayerLevelController : BaseController<PlayerLevelController>
{
    private readonly IMediator _mediator;

    public PlayerLevelController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("getPlayerLevelProfile/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<PlayerLevelProfile>> GetPlayerLevelProfile(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetPlayerLevelCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("updatePlayerLevelRound")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdatePlayerLevelRound([FromBody] BasicCardRequest request)
    {
        var response = await _mediator.Send(new UpdatePlayerLevelRoundCommand(request));
        return response;
    }
}