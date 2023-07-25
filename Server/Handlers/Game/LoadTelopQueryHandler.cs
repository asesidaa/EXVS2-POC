using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record LoadTelopQuery(Request Request) : IRequest<Response>;

public class LoadTelopQueryHandler : IRequestHandler<LoadTelopQuery, Response>
{
    public Task<Response> Handle(LoadTelopQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            load_telop = new Response.LoadTelop
            {
                TelopData = "Test telop"
            }
        });
    }
}