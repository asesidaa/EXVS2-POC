using MediatR;
using nue.protocol.exvs;
using ServerOver.Commands.ClassMatch;
using ServerOver.Commands.ClassMatch.SoloThreshold;
using ServerOver.Commands.ClassMatch.TeamThreshold;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game;

public record LoadClassMatchCommand(Request Request) : IRequest<Response>;

public class LoadClassMatchCommandHandler : IRequestHandler<LoadClassMatchCommand, Response>
{
    private readonly ILogger<LoadClassMatchCommandHandler> _logger;
    private readonly ServerDbContext _context;

    public LoadClassMatchCommandHandler(ILogger<LoadClassMatchCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public Task<Response> Handle(LoadClassMatchCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        // Approximately:
        // Rate obtained in Win = ConstantRate * [1 + (WinScore / WinScoreCoefficient)],
        // but if you win the one which has significantly lower Rate, the Win Point will be lesser
        // Rate deducted in Loss = ConstantRate, but if you loss to the one which has Higher Rate, the Loss Point will be lesser
        var loadClassMatchResponse = new Response.LoadClassMatch()
        {
            ConstantRate = 10, // Mainly relies on this Rate for both Win and Loss
            WinScoreCoefficient = 25000, // During Class Match Pt calculation, Player Battle Score will be divided by this factor
            ClassBaseWinPoint = 50,
            ClassBaseLosePoint = 10,
            ClassWinResultBonus = 1,
            ClassLoseResultBonus = 0,
            ClassWinBonus = 1,
            SearchRatio1 = 1,
            SearchRatio2 = 2,
            SearchRatio3 = 3,
            NextClassUpdateDate = GetNextMonday(DateTime.Today).ToString("yyyy.%M.dd"),
            OverStartDate = (ulong)(DateTimeOffset.Now - TimeSpan.FromDays(10)).ToUnixTimeSeconds()
        };
        
        loadClassMatchResponse.ClassMatchingTimeSchedules.Add(new Response.LoadClassMatch.ClassMatchingTimeSchedule()
        {
            StartAt = (ulong)(DateTimeOffset.Now - TimeSpan.FromDays(10)).ToUnixTimeSeconds(),
            EndAt = (ulong)(DateTimeOffset.Now + TimeSpan.FromDays(365)).ToUnixTimeSeconds(),
            PatternId = 1
        });
        
        loadClassMatchResponse.ClassMatchingPatternTables.Add(new Response.LoadClassMatch.ClassMatchingPatternTable()
        {
            PatternId = 1
        });
        
        loadClassMatchResponse.ClassMatchingRequestTables.Add(new Response.LoadClassMatch.ClassMatchingRequest()
        {
            MakeRoomFlag = true,
            MatchingTryNum = 1,
            RoomSearchId = 1,
            SeqNum = 1
        });

        var loadClassMatchCommands = new List<IClassMatchFillerCommand>()
        {
            // For 1. Showing current User percentile in Main Screen, and 2. Showing progress bar after battle
            new SoloPilotThresholdFiller(_context),
            new SoloValiantThresholdFiller(_context),
            new SoloAceThresholdFiller(_context),
            new SoloOverThresholdFiller(_context),
            new TeamPilotThresholdFiller(_context),
            new TeamValiantThresholdFiller(_context),
            new TeamAceThresholdFiller(_context),
            new TeamOverThresholdFiller(_context),
            // For Showing Upgrade % and Downgrade % in Main Screen
            new PilotClassChangePercentageCommand(),
            new ValiantClassChangePercentageCommand(),
            new AceClassChangePercentageCommand(),
            new OverClassChangePercentageCommand()
        };
        
        loadClassMatchCommands.ForEach(loadClassMatchCommand => loadClassMatchCommand.Fill(loadClassMatchResponse));
        
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
            load_class_match = loadClassMatchResponse
        };

        
        return Task.FromResult(response);
    }
    
    DateTime GetNextMonday(DateTime start)
    {
        int daysToAdd = ((int) DayOfWeek.Monday - (int) start.DayOfWeek + 7) % 7;
        return start.AddDays(daysToAdd);
    }
}