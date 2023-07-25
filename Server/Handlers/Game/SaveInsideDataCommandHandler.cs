using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record SaveInsideDataCommand(Request Request) : IRequest<Response>;


public class SaveInsideDataCommandHandler : IRequestHandler<SaveInsideDataCommand, Response>
{
    public Task<Response> Handle(SaveInsideDataCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_inside_data = new Response.SaveInsideData()
        });
    }
}
