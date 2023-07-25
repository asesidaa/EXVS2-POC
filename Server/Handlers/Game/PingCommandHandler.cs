using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record PingCommand(Request Request) : IRequest<Response>;

public class PingCommandHandler : IRequestHandler<PingCommand, Response>
{
    public Task<Response> Handle(PingCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            ping = new Response.Ping
            {
                GameServer = true,
                AcidServer = false,
                MatchmakingServer = false,
                ResponseAt = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds()
            }
        });
    }
}