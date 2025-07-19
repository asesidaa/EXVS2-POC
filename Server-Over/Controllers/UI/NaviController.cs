using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerOver.Handlers.UI.Navi;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;
using WebUIOVer.Shared.Dto.Response;

namespace ServerOver.Controllers.UI;

[ApiController]
[Route("ui/navi")]
public class NaviController : BaseController<NaviController>
{
    private readonly IMediator _mediator;

    public NaviController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("getNaviProfile/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<NaviProfile>> GetNaviProfile(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetNaviProfileCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("upsertNaviProfile")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertNaviProfile([FromBody] UpsertNaviProfileRequest request)
    {
        var response = await _mediator.Send(new UpsertNaviProfileCommand(request));
        return response;
    }
    
    [HttpPost("updateAllNaviCostume")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateAllNaviCostume([FromBody] UpdateAllNaviCostumeRequest request)
    {
        var response = await _mediator.Send(new UpdateAllNaviCostumeCommand(request));
        return response;
    }
}