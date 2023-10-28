using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Handlers.Card.Battle;
using WebUI.Shared.Dto.Common;

namespace Server.Controllers;

[ApiController]
[Route("battle-analysis")]
public class AnalysisController : BaseController<AnalysisController>
{
    private readonly IMediator mediator;
    
    public AnalysisController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpGet("getSelfUsage/{accessCode}/{chipId}/{mode}")]
    [Produces("application/json")]
    public async Task<ActionResult<Usage>> GetSelfUsage(string accessCode, string chipId, string mode)
    {
        var response = await mediator.Send(new GetSelfUsageCommand(accessCode, chipId, mode));
        return response;
    }
    
    [HttpGet("getAgainstMsWinLossRecord/{accessCode}/{chipId}/{mode}")]
    [Produces("application/json")]
    public async Task<ActionResult<List<MsBattleRecord>>> GetAgainstMsWinLossRecord(string accessCode, string chipId, string mode)
    {
        var response = await mediator.Send(new GetAgainstMsWinLossRecordCommand(accessCode, chipId, mode));
        return response;
    }
    
    [HttpGet("getAllUsage/{mode}")]
    [Produces("application/json")]
    public async Task<ActionResult<Usage>> GetAllUsage(string mode)
    {
        var response = await mediator.Send(new GetAllUsageCommand(mode));
        return response;
    }
}