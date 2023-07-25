using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record CheckCommunicationQuery(Request Request) : IRequest<Response>;

public class CheckCommunicationQueryHandler : IRequestHandler<CheckCommunicationQuery, Response>
{
    public Task<Response> Handle(CheckCommunicationQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            check_communication = new Response.CheckCommunication()
        });
    }
}