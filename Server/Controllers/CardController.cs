using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Handlers.Card;
using Server.Handlers.Card.Navi;
using Server.Handlers.Card.Profile;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;

namespace Server.Controllers;

[ApiController]
[Route("card")]
public class CardController : BaseController<CardController>
{
    private readonly IMediator mediator;
    
    public CardController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpGet("getAll")]
    [Produces("application/json")]
    public async Task<ActionResult<List<BareboneCardProfile>>> GetAll()
    {
        var response = await mediator.Send(new GetAllBareboneCardCommand());
        return response;
    }
    
    [HttpGet("getBasicDisplayProfile/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicDisplayProfile>> GetBasicDisplayProfile(String accessCode, String chipId)
    {
        var response = await mediator.Send(new GetBasicDisplayProfileCommand(accessCode, chipId));
        return response;
    }
    
    [HttpGet("getNaviProfile/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<NaviProfile>> GetNaviProfile(String accessCode, String chipId)
    {
        var response = await mediator.Send(new GetNaviProfileCommand(accessCode, chipId));
        return response;
    }

    [HttpPost("upsertDefaultNavi")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertDefaultNavi([FromBody] UpsertDefaultNaviRequest request)
    {
        var response = await mediator.Send(new UpsertDefaultNaviCommand(request));
        return response;
    }
    
    [HttpPost("upsertNaviCostume")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertNaviCostume([FromBody] UpsertNaviCostumeRequest request)
    {
        var response = await mediator.Send(new UpsertNaviCostumeCommand(request));
        return response;
    }
}