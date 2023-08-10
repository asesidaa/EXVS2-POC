using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Handlers.Upload;

namespace Server.Controllers;

[ApiController]
[Route("upload")]
[ApiExplorerSettings(IgnoreApi = true)]
public class UploadController : BaseController<UploadController>
{
    private readonly IMediator mediator;

    public UploadController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpPut("uploadImage/{cardId}/{accessToken}")]
    public async Task<ActionResult<string>> UploadImage(String cardId, String accessToken)
    {
        var response = await mediator.Send(new UploadImageCommand(cardId, accessToken, Request));
        return response;
    }
}