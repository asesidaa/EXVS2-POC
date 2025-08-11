using MediatR;
using ServerOver.Dtos.Request.AmActivator;
using ServerOver.Dtos.Response.AmActivator;

namespace ServerOver.Controllers.AmActivator;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("mucha_activation")]
public class ActivatorController
{
    private readonly IMediator _mediator;

    public ActivatorController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("otk")]
    [Produces("application/json")]
    // This API seemingly only triggered in Test Menu -> Network -> Activation -> One Time Key
    public OneTimeKeyResponse ObtainOneTimeKey([FromBody] OneTimeKeyRequest oneTimeKeyRequest)
    {
        return new OneTimeKeyResponse()
        {
            Status = 200,
            Message = string.Empty,
            Otk = 80652673,
            Uuid = "b6229b54-e11c-4d41-914a-674c17f6c839",
            ExpiredAt = "2029-01-15T09:49:56Z"
        };
    }
    
    [HttpPost("signature")]
    [Produces("application/json")]
    // This API seemingly only triggered in Test Menu -> Network -> Activation -> One Time Key
    public SignatureResponse ObtainSignature([FromBody] SignatureRequest signatureRequest)
    {
        return new SignatureResponse()
        {
            Status = 200,
            Message = string.Empty,
            Signature = 1
        };
    }
}