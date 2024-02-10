using MediatR;
using nue.protocol.exvs;
using ServerVanilla.Command.SaveBattle;
using ServerVanilla.Command.SaveBattle.Common;
using ServerVanilla.Command.SaveBattle.PvP;
using ServerVanilla.Mapper.Context;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Enum;

namespace ServerVanilla.Handlers.Game;

public record SaveVsmResultCommand(Request Request) : IRequest<Response>;

public class SaveVsmResultCommandHandler : IRequestHandler<SaveVsmResultCommand, Response>
{
    private readonly ILogger<SaveVsmResultCommandHandler> _logger;
    private readonly ServerDbContext _context;

    public SaveVsmResultCommandHandler(ILogger<SaveVsmResultCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public Task<Response> Handle(SaveVsmResultCommand request, CancellationToken cancellationToken)
    {
        var sessionId = request.Request.save_vsm_result.SessionId;
        var cardId = request.Request.save_vsm_result.PilotId;

        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.SessionId == sessionId && x.Id == cardId);

        if (cardProfile == null)
        {
            return Task.FromResult(new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.Success,
                save_vsm_result = new Response.SaveVsmResult()
            });
        }

        var isShuffle = request.Request.save_vsm_result.ShuffleFlag;
        var playResultGroup = request.Request.save_vsm_result.Result;
        var battleResultContext = playResultGroup.ToBattleResultContext();

        battleResultContext.CommonDomain.BattleMode = isShuffle ? BattleModeConstant.OfflineSolo : BattleModeConstant.OfflineTeam;

        playResultGroup.Foes
            .Where(foe => foe.CpuFlag == 0)
            .ToList()
            .ForEach(foe =>
            {
                battleResultContext.BattleStatisticDomain.TotalEnemyDefeatedCount += foe.DownNum;
            });
        
        var saveBattleDataCommands = new List<ISaveBattleDataCommand>()
        {
            // Common Commands
            new SaveGpCommand(_context),
            new SaveEchelonCommand(_context),
            new SaveNaviCommand(_context),
            new SaveMobileSuitMasteryCommand(_context),
            new SaveTeamCommand(_context),
            // PvP Commands
            new SaveWinLossRecordCommand(_context)
        };
        
        saveBattleDataCommands.ForEach(command => command.Save(cardProfile, battleResultContext));
        
        _context.SaveChanges();
        
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsm_result = new Response.SaveVsmResult()
        });
    }
}
