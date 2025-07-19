using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerOver.Commands.SaveBattle;
using ServerOver.Commands.SaveBattle.Common;
using ServerOver.Commands.SaveBattle.Triad;
using ServerOver.Commands.SaveBattle.Triad.Ranking;
using ServerOver.Mapper.Context;
using ServerOver.Models.Config;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Enum;

namespace ServerOver.Handlers.Game;

public record SaveVscResultCommand(Request Request) : IRequest<Response>;

public class SaveVscResultCommandHandler : IRequestHandler<SaveVscResultCommand, Response>
{
    private readonly ILogger<SaveVscResultCommandHandler> _logger;
    private readonly ServerDbContext _context;
    private readonly CardServerConfig _config;

    public SaveVscResultCommandHandler(ILogger<SaveVscResultCommandHandler> logger, ServerDbContext context, IOptions<CardServerConfig> options)
    {
        _logger = logger;
        _context = context;
        _config = options.Value;
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

        var currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
        var currentYear = (uint) currentTime.Year;
        var currentMonth = (uint) currentTime.Month;
        
        var playResultGroup = request.Request.save_vsc_result.Result;
        var battleResultContext = playResultGroup.ToBattleResultContext();

        battleResultContext.CommonDomain.BattleMode = BattleModeConstant.Triad;
        
        var saveBattleDataCommands = new List<ISaveBattleDataCommand>()
        {
            // Common Commands
            new SaveGpCommand(_context),
            new SavePlayerLevelCommand(_context),
            new SaveNaviCommand(_context),
            new SaveMobileSuitMasteryCommand(_context),
            new SaveTeamCommand(_context),
            // Triad Mode Exclusive Commands
            new SaveTriadPartnerCommand(_context),
            new SaveTriadMiscInfoCommand(_context),
            new SaveTriadCourseScoreCommand(_context),
            new AddReleaseTriadCourseCommand(_context),
            // Triad Ranking Commands
            new SaveTriadRankTargetCommand(_context, currentYear, currentMonth),
            new SaveTriadRankWantedCommand(_context, currentYear, currentMonth),
            new SaveTriadRankHighScoreCommand(_context, _config, currentYear, currentMonth),
            new SaveTriadRankClearTimeCommand(_context, _config, currentYear, currentMonth)
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
