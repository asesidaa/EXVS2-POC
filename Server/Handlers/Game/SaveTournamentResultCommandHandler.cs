using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record SaveTournamentResultCommand(Request Request) : IRequest<Response>;

public class SaveTournamentResultCommandHandler : IRequestHandler<SaveTournamentResultCommand, Response>
{
    public Task<Response> Handle(SaveTournamentResultCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_tournament_result = new Response.SaveTournamentResult()
        });
    }
}