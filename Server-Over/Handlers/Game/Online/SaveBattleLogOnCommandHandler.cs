using MediatR;
using nue.protocol.exvs;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game.Online;

public record SaveBattleLogOnCommand(Request Request) : IRequest<Response>;

public class SaveBattleLogOnCommandHandler : IRequestHandler<SaveBattleLogOnCommand, Response>
{
    private readonly ILogger<SaveBattleLogOnCommandHandler> _logger;
    private readonly ServerDbContext _context;
    
    public SaveBattleLogOnCommandHandler(ILogger<SaveBattleLogOnCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public Task<Response> Handle(SaveBattleLogOnCommand request, CancellationToken cancellationToken)
    {
        var successResponse = new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_battle_log_on = new Response.SaveBattleLogOn()
        };
        
        return Task.FromResult(successResponse);
    }
}
