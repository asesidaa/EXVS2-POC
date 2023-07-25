using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record SaveVstResultCommand(Request Request) : IRequest<Response>;

public class SaveVstResultCommandHandler : IRequestHandler<SaveVstResultCommand, Response>
{
    public Task<Response> Handle(SaveVstResultCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vst_result = new Response.SaveVstResult()
        });
    }
}
