using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record SaveBattleLogOnCommand(Request Request) : IRequest<Response>;

public class SaveBattleLogOnCommandHandler : IRequestHandler<SaveBattleLogOnCommand, Response>
{
    public Task<Response> Handle(SaveBattleLogOnCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_battle_log_on = new Response.SaveBattleLogOn()
        });
    }
}
