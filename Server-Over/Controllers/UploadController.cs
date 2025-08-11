using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerOver.Handlers.Upload;

namespace ServerOver.Controllers;

[ApiController]
[Route("upload")]
[ApiExplorerSettings(IgnoreApi = true)]
public class UploadController : BaseController<UploadController>
{
    private readonly IMediator _mediator;

    public UploadController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("uploadImage/{cardId}/{accessToken}")]
    public async Task<ActionResult<string>> UploadImage(String cardId, String accessToken)
    {
        var response = await _mediator.Send(new UploadImageCommand(cardId, accessToken, Request));
        return response;
    }
    
    [HttpPut("uploadReplay/{playerId}/{replayTime}")]
    public async Task<ActionResult<string>> UploadReplay(String playerId, String replayTime)
    {
        var response = await _mediator.Send(new UploadReplayCommand(playerId, replayTime, Request));
        return response;
    }
}