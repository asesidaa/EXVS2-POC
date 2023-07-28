using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Dto.Request;
using Server.Dto.Response;
using Server.Handlers.Card;

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

    [HttpPost("upsertDefaultNavi")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertDefaultNavi([FromBody] UpsertDefaultNaviRequest request)
    {
        uint defaultBattleNaviId = request.defaultBattleNaviId;
        var responseBody = new BasicResponse
        {
            success = true
        };
        return null;
    }
}