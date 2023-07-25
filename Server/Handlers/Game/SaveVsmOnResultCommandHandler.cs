using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record SaveVsmOnResultCommand(Request Request) : IRequest<Response>;

public class SaveVsmOnResultCommandHandler : IRequestHandler<SaveVsmOnResultCommand, Response>
{
    public Task<Response> Handle(SaveVsmOnResultCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsm_on_result = new Response.SaveVsmOnResult()
        });
    }
}
