using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerOver.Handlers.UI.Message;
using WebUIOver.Shared.Dto.Message;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Controllers.UI;

[ApiController]
[Route("ui/message")]
public class MessageController : BaseController<MessageController>
{
    private readonly IMediator _mediator;

    public MessageController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("getCustomMessageGroupSetting/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<CustomMessageGroupSetting>> GetCustomMessageGroupSetting(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetCustomMessageGroupSettingCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("upsertCustomMessages")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertCustomMessages([FromBody] UpsertCustomMessagesRequest request)
    {
        var response = await _mediator.Send(new UpsertCustomMessagesCommand(request));
        return response;
    }
}