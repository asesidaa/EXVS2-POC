using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record RegisterPcbAckCommand(Request Request) : IRequest<Response>;

public class RegisterPcbAckCommandHandler : IRequestHandler<RegisterPcbAckCommand, Response>
{
    public Task<Response> Handle(RegisterPcbAckCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            register_pcb_ack = new Response.RegisterPcbAck()
        });
    }
}