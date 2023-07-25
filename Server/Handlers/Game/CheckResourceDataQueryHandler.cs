using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record CheckResourceDataQuery(Request Request) : IRequest<Response>;

public class CheckResourceDataQueryHandler : IRequestHandler<CheckResourceDataQuery, Response>
{
    public Task<Response> Handle(CheckResourceDataQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            check_resource_data = new Response.CheckResourceData()
        });
    }
}