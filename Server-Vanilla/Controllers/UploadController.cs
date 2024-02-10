using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerVanilla.Handlers.Upload;

namespace ServerVanilla.Controllers;

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
    
    [HttpPut("uploadReplay/{playerId}/{replayTime}")]
    public async Task<ActionResult<string>> UploadReplay(String playerId, String replayTime)
    {
        var response = await _mediator.Send(new UploadReplayCommand(playerId, replayTime, Request));
        return response;
    }
}