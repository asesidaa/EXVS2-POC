using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerOver.Handlers.UI.Triad;
using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Dto.Triad;
using WebUIOver.Shared.Validator;

namespace ServerOver.Controllers.UI;

[ApiController]
[Route("ui/triad")]
public class TriadController : BaseController<TriadController>
{
    private readonly IMediator _mediator;

    public TriadController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("getCpuTriadPartner/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<CpuTriadPartner>> GetCpuTriadPartner(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetCpuTriadPartnerCommand(accessCode, chipId));
        return response;
    }

    [HttpPost("updateCpuTriadPartner")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateCpuTriadPartner(
        [FromBody] UpdateCpuTriadPartnerRequest request)
    {
        bool validateResult = new CpuTriadPartnerValidator().Validate(request.CpuTriadPartner);

        if (!validateResult)
        {
            return BadRequest(
                new ErrorResponse
                {
                    ErrorMsg =
                        "Please double check the level... Each of them are maxed at 100, and total is maxed at 500"
                });
        }

        var response = await _mediator.Send(new UpdateCpuTriadPartnerCommand(request));
        return response;
    }
    
    [HttpGet("getTriadCourseOverallResult/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<TriadCourseOverallResult>> GetTriadCourseResults(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetTriadCourseResultsCommand(accessCode, chipId));
        return response;
    }
}