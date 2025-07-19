using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerOver.Handlers.UI.Training;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Dto.Training;

namespace ServerOver.Controllers.UI;

[ApiController]
[Route("ui/training")]
public class TrainingController : BaseController<MessageController>
{
    private readonly IMediator _mediator;

    public TrainingController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("get/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<TrainingProfile>> GetTrainingProfile(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetTrainingProfileCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("save")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertGamepadConfig([FromBody] UpdateTrainingProfileRequest request)
    {
        var response = await _mediator.Send(new UpsertTrainingProfileCommand(request));
        return response;
    }
}