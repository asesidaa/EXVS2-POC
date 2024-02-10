using MediatR;
using nue.protocol.exvs;
using ServerVanilla.Command.SaveBattle;
using ServerVanilla.Command.SaveBattle.Common;
using ServerVanilla.Command.SaveBattle.Triad;
using ServerVanilla.Mapper.Context;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Enum;

namespace ServerVanilla.Handlers.Game;

public record SaveVscResultCommand(Request Request) : IRequest<Response>;

public class SaveVscResultCommandHandler : IRequestHandler<SaveVscResultCommand, Response>
{
    private readonly ILogger<SaveVscResultCommandHandler> _logger;
    private readonly ServerDbContext _context;

    public SaveVscResultCommandHandler(ILogger<SaveVscResultCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public Task<Response> Handle(SaveVscResultCommand request, CancellationToken cancellationToken)
    {
        var sessionId = request.Request.save_vsc_result.SessionId;
        var cardId = request.Request.save_vsc_result.PilotId;

        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.SessionId == sessionId && x.Id == cardId);

        if (cardProfile == null)
        {
            return Task.FromResult(new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.Success,
                save_vsc_result = new Response.SaveVscResult()
            });
        }

        var playResultGroup = request.Request.save_vsc_result.Result;
        var battleResultContext = playResultGroup.ToBattleResultContext();

        battleResultContext.CommonDomain.BattleMode = BattleModeConstant.Triad;
        
        var saveBattleDataCommands = new List<ISaveBattleDataCommand>()
        {
            // Common Commands
            new SaveGpCommand(_context),
            new SaveEchelonCommand(_context),
            new SaveNaviCommand(_context),
            new SaveMobileSuitMasteryCommand(_context),
            new SaveTeamCommand(_context),
            // Triad Mode Exclusive Commands
            new SaveTriadMiscInfoCommand(_context),
            new SaveTriadCourseScoreCommand(_context),
            new AddReleaseTriadCourseCommand(_context)
        };
        
        saveBattleDataCommands.ForEach(command => command.Save(cardProfile, battleResultContext));
        
        _context.SaveChanges();
        
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsc_result = new Response.SaveVscResult()
        });
    }
}
