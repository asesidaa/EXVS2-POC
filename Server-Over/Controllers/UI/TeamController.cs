using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerOver.Handlers.UI.Team;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Controllers.UI;

[ApiController]
[Route("ui/team")]
public class TeamController : BaseController<TeamController>
{
    private readonly IMediator _mediator;

    public TeamController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("getTeams/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<TeamResponse>> GetTeams(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetCustomizeTeamCommand(accessCode, chipId));
        return response;
    }

    [HttpGet("checkPlayerExistence/{partnerId}/{partnerToken}")]
    [Produces("application/json")]
    public async Task<ActionResult<PlayerExistenceResult>> CheckPlayerExistence(uint partnerId, String partnerToken)
    {
        var response = await _mediator.Send(new CheckPlayerExistenceResultCommand(partnerId, partnerToken));
        return response;
    }
    
    [HttpPost("preCreateTeam")]
    [Produces("application/json")]
    public async Task<ActionResult<PreCreateTeamResponse>> PreCreateTeam([FromBody] PreCreateTeamRequest request)
    {
        var response = await _mediator.Send(new PreCreateTeamCommand(request));
        return response;
    }

    [HttpPost("upsertTeams")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertTeams([FromBody] UpsertTeamsRequest request)
    {
        var response = await _mediator.Send(new UpsertCustomizeTeamCommand(request));
        return response;
    }
}