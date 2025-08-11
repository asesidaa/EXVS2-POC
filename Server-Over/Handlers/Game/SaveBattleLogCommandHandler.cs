using MediatR;
using nue.protocol.exvs;
using ServerOver.Models.Cards.Battle.History;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game;

public record SaveBattleLogCommand(Request Request) : IRequest<Response>;

public class SaveBattleLogCommandHandler : IRequestHandler<SaveBattleLogCommand, Response>
{
    private readonly ILogger<SaveBattleLogCommandHandler> _logger;
    private readonly ServerDbContext _context;
    
    public SaveBattleLogCommandHandler(ILogger<SaveBattleLogCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public Task<Response> Handle(SaveBattleLogCommand request, CancellationToken cancellationToken)
    {
        var successResponse = new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_battle_log = new Response.SaveBattleLog()
        };

        var saveBattleLogRequest = request.Request.save_battle_log;
        var sessionId = saveBattleLogRequest.SessionId;
        
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.SessionId == sessionId);

        if (cardProfile == null)
        {
            return Task.FromResult(successResponse);
        }

        var oldPreBattleHistory = _context.PreBattleHistoryDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile);

        if (oldPreBattleHistory is not null)
        {
            _context.PreBattleHistoryDbSet.Remove(oldPreBattleHistory);
            _context.SaveChanges();
        }

        if (saveBattleLogRequest.BattleLog.GameMode != GameMode.GmodeNone)
        {
            return Task.FromResult(successResponse);
        }

        var ownPlayer = saveBattleLogRequest.BattleLog.Pilots
            .FirstOrDefault(x => x.IsHuman && x.PilotId == cardProfile.Id);

        if (ownPlayer is null)
        {
            return Task.FromResult(successResponse);
        }

        var newPreBattleHistory = new PreBattleHistory()
        {
            CurrentConsecutiveWins = ownPlayer.ConsecutiveWin,
            CardId = cardProfile.Id,
            CardProfile = cardProfile
        };

        _context.PreBattleHistoryDbSet.Add(newPreBattleHistory);
        _context.SaveChanges();
        
        return Task.FromResult(successResponse);
    }
}
