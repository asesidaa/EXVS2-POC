using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record RegisterCardCommand(Request Request) : IRequest<Response>;

public class RegisterCardCommandHandler : IRequestHandler<RegisterCardCommand, Response>
{
    public Task<Response> Handle(RegisterCardCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
            register_card = new Response.RegisterCard
            {
                SessionId = request.register_card.SessionId,
                AccessCode = request.register_card.AccessCode,
                AcidError = AcidError.AcidNoUse,
                IsRegistered = false,
                PilotId = 1
            }
        };
        return Task.FromResult(response);
    }
}