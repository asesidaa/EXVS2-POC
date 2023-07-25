using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record SaveBattleLogCommand(Request Request) : IRequest<Response>;

public class SaveBattleLogCommandHandler : IRequestHandler<SaveBattleLogCommand, Response>
{
    public Task<Response> Handle(SaveBattleLogCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_battle_log = new Response.SaveBattleLog()
        });
    }
}
