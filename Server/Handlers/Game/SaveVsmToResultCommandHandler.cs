using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record SaveVsmToResultCommand(Request Request) : IRequest<Response>;

public class SaveVsmToResultCommandHandler : IRequestHandler<SaveVsmToResultCommand, Response>
{
    public Task<Response> Handle(SaveVsmToResultCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsm_to_result = new Response.SaveVsmToResult()
        });
    }
}
