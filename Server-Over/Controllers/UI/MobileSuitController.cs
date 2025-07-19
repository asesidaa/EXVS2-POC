using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerOver.Handlers.UI.MobileSuit;
using WebUIOVer.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Controllers.UI;

[ApiController]
[Route("ui/mobileSuit")]
public class MobileSuitController : BaseController<MobileSuitController>
{
    private readonly IMediator _mediator;

    public MobileSuitController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("getAllFavouriteMs/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<List<FavouriteMs>>> GetAllFavouriteMs(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetAllFavouriteMsCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("updateAllFavouriteMs")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateAllFavouriteMs([FromBody] UpdateAllFavouriteMsRequest request)
    {
        var response = await _mediator.Send(new UpdateAllFavouriteMsCommand(request));
        return response;
    }
    
    [HttpGet("getUsedMobileSuitData/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<List<MsSkillGroup>>> GetUsedMobileSuitData(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetUsedMobileSuitDataCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("updateAllMsCostume")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateAllMsCostume([FromBody] UpdateAllMsCostumeSkinRequest request)
    {
        var response = await _mediator.Send(new UpdateAllMsCostumeRequestCommand(request));
        return response;
    }
    
    [HttpPost("updateAllMsSkin")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateAllMsSkin([FromBody] UpdateAllMsCostumeSkinRequest request)
    {
        var response = await _mediator.Send(new UpdateAllMsSkinRequestCommand(request));
        return response;
    }
}