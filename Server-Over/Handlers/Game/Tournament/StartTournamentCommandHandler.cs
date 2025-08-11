using MediatR;
using nue.protocol.exvs;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game.Tournament;

public record StartTournamentCommand(Request Request) : IRequest<Response>;

public class StartTournamentCommandHandler : IRequestHandler<StartTournamentCommand, Response>
{
    private readonly ILogger<StartTournamentCommandHandler> _logger;
    private readonly ServerDbContext _context;

    public StartTournamentCommandHandler(ILogger<StartTournamentCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task<Response> Handle(StartTournamentCommand query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
            start_tournament = new Response.StartTournament()
            {
                TournamentId = 1
            }
        };
        
        return Task.FromResult(response);
    }
}