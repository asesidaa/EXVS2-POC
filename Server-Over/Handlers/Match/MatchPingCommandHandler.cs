using MediatR;
using nue.protocol.mms;

namespace ServerOver.Handlers.Match;

public record MatchPingCommand(Request Request) : IRequest<Response>;

public class MatchPingCommandHandler : IRequestHandler<MatchPingCommand, Response>
{
    public Task<Response> Handle(MatchPingCommand request, CancellationToken cancellationToken)
    {
        var response = new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Code = ErrorCode.Success,
            ping = new Response.Ping()
        };

        return Task.FromResult(response);
    }
}