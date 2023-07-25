using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record CheckTelopQuery(Request Request) : IRequest<Response>;

public class CheckTelopQueryHandler : IRequestHandler<CheckTelopQuery, Response>
{
    public Task<Response> Handle(CheckTelopQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            check_telop = new Response.CheckTelop
            {
                Telop1Id = 1
            }
        });
    }
}