using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record SaveLogCommand(Request Request) : IRequest<Response>;

public class SaveLogCommandHandler : IRequestHandler<SaveLogCommand, Response>
{
    public Task<Response> Handle(SaveLogCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_log = new Response.SaveLog
            {
                LoadGameDataVer = 1
            }
        });
    }
}