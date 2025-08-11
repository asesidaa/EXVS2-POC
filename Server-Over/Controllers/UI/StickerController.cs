using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerOver.Handlers.UI.Sticker;
using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Request.Sticker;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Controllers.UI;

[ApiController]
[Route("ui/sticker")]
public class StickerController : BaseController<StickerController>
{
    private readonly IMediator _mediator;

    public StickerController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("getDefaultSticker/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<StickerDto>> GetDefaultSticker(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetDefaultStickerCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("updateDefaultSticker")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateDefaultSticker([FromBody] UpdateDefaultStickerRequest request)
    {
        var response = await _mediator.Send(new UpdateDefaultStickerCommand(request));
        return response;
    }
    
    [HttpGet("getMobileSuitStickers/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<List<StickerDto>>> GetMobileSuitStickers(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetMobileSuitStickerCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("upsertMobileSuitStickers")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertMobileSuitStickers([FromBody] UpsertMobileSuitStickersRequest request)
    {
        var response = await _mediator.Send(new UpsertMobileSuitStickersCommand(request));
        return response;
    }
}