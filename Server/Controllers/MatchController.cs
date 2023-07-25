using Microsoft.AspNetCore.Mvc;
using nue.protocol.mms;
using Swan.Formatters;

namespace Server.Controllers;

public class MatchController : BaseController<MatchController>
{
    [Route("match")]
    [HttpPost]
    [Produces("application/protobuf")]
    public IActionResult Match([FromBody] Request request)
    {
        Logger.LogInformation("Request is {Request}", request.Stringify());
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Code = ErrorCode.Success
        };
        switch (request.Type)
        {
            case MethodType.Ping:
                response.ping = new Response.Ping();
                return Ok(response);
            case MethodType.IssueNodeId:
                response.issue_node_id = new Response.IssueNodeId
                {
                    NodeIds = new []{1u}
                };
                return Ok(response);
            case MethodType.EntryMatching:
            case MethodType.CheckMatching:
            case MethodType.CancelMatching:
            case MethodType.MatchingSetting:
            default:
                return NotFound();
        }
    }
}