using MediatR;
using nue.protocol.exvs;
using ServerOver.Commands.SaveBattle;
using ServerOver.Commands.SaveBattle.Common;
using ServerOver.Commands.SaveBattle.PvP;
using ServerOver.Mapper.Context;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Enum;

namespace ServerOver.Handlers.Game;

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

        var ally = playResultGroup.Partner;

        if (ally is not null)
        {
            if (ally.CpuFlag == 0)
            {
                battleResultContext.BattleHistoryDomain.Ally = ally;
            }
        }

        battleResultContext.BattleHistoryDomain.PlayerActions = playResultGroup.PlayerBattleLogs;
        
        battleResultContext.BattleHistoryDomain.FilteredTargets = playResultGroup.Foes
            .Where(foe => foe.CpuFlag == 0)
            .ToList();
        
        battleResultContext.BattleHistoryDomain.FilteredTargets
            .ForEach(foe =>
            {
                battleResultContext.BattleStatisticDomain.TotalEnemyDefeatedCount += foe.DownNum;
            });
        
        var saveBattleDataCommands = new List<ISaveBattleDataCommand>()
        {
            // Common Commands
            new SaveGpCommand(_context),
            new SavePlayerLevelCommand(_context),
            new SaveNaviCommand(_context),
            new SaveMobileSuitMasteryCommand(_context),
            new SaveTeamCommand(_context),
            // PvP Commands
            new SaveWinLossRecordCommand(_context),
            new SavePlayerStatisticCommand(_context),
            new SaveChallengeMissionDataCommand(_context),
            new SaveMobileSuitTrackerStatCommand(_context),
            new SaveBurstTypeCommand(_context),
            new SaveBattleHistoryCommand(_context),
            new RemovePreBattleHistoryCommand(_context)
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
