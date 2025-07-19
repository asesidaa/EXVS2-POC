using MediatR;
using Microsoft.AspNetCore.Mvc;
using nue.protocol.mms;
using ServerOver.Handlers.Match;
using Swan.Formatters;

namespace ServerOver.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class MatchController : BaseController<MatchController>
{
    private readonly IMediator _mediator;
    
    public MatchController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Route("match")]
    [HttpPost]
    [Produces("application/protobuf")]
    public async Task<IActionResult> Match([FromBody] Request request)
    {
        Logger.LogInformation("Request is {Request}", request.Stringify());
        
        var response = request.Type switch
        {
            MethodType.Ping => await _mediator.Send(new MatchPingCommand(request)),
            MethodType.IssueNodeId => await _mediator.Send(new MatchIssueNodeIdCommand(request)),
            MethodType.MatchingSetting => await _mediator.Send(new MatchingSettingCommand(request)),
            _ => UnhandledResponse(request)
        };
        
        return Ok(response);
    }
    
    private Response UnhandledResponse(Request request)
    {
        return new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Code = ErrorCode.ErrServer
        };
    }
}