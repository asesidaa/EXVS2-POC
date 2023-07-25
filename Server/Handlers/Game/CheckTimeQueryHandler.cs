using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record CheckTimeQuery(Request Request) : IRequest<Response>;


public class CheckTimeQueryHandler : IRequestHandler<CheckTimeQuery, Response>
{
    public Task<Response> Handle(CheckTimeQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            check_time = new Response.CheckTime
            {
                At = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds()
            }
        });
    }
}