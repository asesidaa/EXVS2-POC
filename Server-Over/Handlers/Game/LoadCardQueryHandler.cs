using MediatR;
using nue.protocol.exvs;
using ServerOver.Commands.LoadCard.MobileUser;
using ServerOver.Commands.LoadCard.PilotData;
using ServerOver.Commands.LoadCard.PostProcess;
using ServerOver.Constants;
using ServerOver.Context.Tracker;
using ServerOver.Persistence;
using ServerOver.Processor.Tracker;
using ServerOver.Processor.Tracker.Rarity;
using ServerOver.Strategy.Team;

namespace ServerOver.Handlers.Game;

public record LoadCardQuery(Request Request) : IRequest<Response>;

public class LoadCardQueryHandler : IRequestHandler<LoadCardQuery, Response>
{
    private readonly ILogger<LoadCardQueryHandler> _logger;
    private readonly ServerDbContext _context;

    public LoadCardQueryHandler(ILogger<LoadCardQueryHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task<Response> Handle(LoadCardQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;

        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
        };

        var sessionId = request.load_card.SessionId;
        
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.SessionId == sessionId);

        if (cardProfile == null)
        {
            response.Error = Error.ErrServer;
            return Task.FromResult(response);
        }
        
        var pilotDataGroup = new Response.LoadCard.PilotDataGroup()
        {
            RewardMsIds = null,
            pilot_class_match = new Response.LoadCard.PilotDataGroup.PilotClassMatch()
        };

        var playerLeaderTeamAppendStrategy = new PlayerLeaderTeamAppendStrategy(_context);
        var playerPartnerTeamAppendStrategy = new PlayerPartnerTeamAppendStrategy(_context);
        
        var pilotDataCommands = new List<IPilotDataCommand>()
        {
            new PilotMobileSuitUsageCommand(_context),
            new TriadMiscInfoCommand(_context),
            new TriadCourseDataCommand(_context),
            new TriadTimeAttackRankRibbonCommand(_context),
            new TriadHighScoreRankRibbonCommand(_context),
            new TriadTargetDestroyRankRibbonCommand(_context),
            new TriadWantedRankRibbonCommand(_context),
            new TrainingDataCommand(_context),
            new SoloClassMatchCommand(_context),
            new TeamClassMatchCommand(_context),
            new LicenseScoreCommand(_context),
            new AppendTeamCommand(playerLeaderTeamAppendStrategy),
            new AppendTeamCommand(playerPartnerTeamAppendStrategy)
        };
        
        pilotDataCommands.ForEach(command => command.Fill(cardProfile, pilotDataGroup));

        var mobileUserGroup = new Response.LoadCard.MobileUserGroup()
        {
            MobileUserId = (uint) cardProfile.Id,
            PaidFlag = LoadCardMobileUserConstants.PaidFlag,
            PlayingStampFlag = LoadCardMobileUserConstants.PlayingStampFlag,
            SupportTicketRemains = LoadCardMobileUserConstants.SupportTicketRemains,
            online_tag_info = new Response.LoadCard.MobileUserGroup.OnlineTagInfo()
        };
        
        var pilotTrackerContextGenerator = new PilotTrackerContextGenerator(_context);
        var pilotTrackerContext = pilotTrackerContextGenerator.Generate(cardProfile);
        var rarityProcessor = new OverboostRarityProcessor();
        var pilotTrackerProcessor = new PilotTrackerProcessor(pilotTrackerContext, rarityProcessor);
        var msTrackerProcessor = new MobileSuitTrackerProcessor(pilotTrackerProcessor);

        var mobileUserCommands = new List<ILoadCardMobileUserCommand>()
        {
            new LoadCustomizeGroupCommand(_context),
            new LoadTriadPartnerCommand(_context),
            new RadarSettingCommand(_context),
            new LoadGamepadCommand(_context),
            new LoadTitleCommand(_context),
            new LoadMessageCommand(_context),
            new OnlinePairCommand(_context),
            new DefaultStickerCommand(_context, pilotTrackerProcessor),
            new MobileSuitStickerCommand(_context, pilotTrackerProcessor, msTrackerProcessor),
            new QuickTagCommand(_context)
        };
        
        mobileUserCommands.ForEach(command => command.Fill(cardProfile, mobileUserGroup));

        var postProcessCommands = new List<ILoadCardPostProcessCommand>()
        {
            new ExTutorialCommand(_context),
            new LatestPlayTimeUpdateCommand(_context)
        };
        
        postProcessCommands.ForEach(command => command.PostProcess(cardProfile, request.load_card));
        
        response.load_card = new Response.LoadCard
        {
            pilot_data_group = pilotDataGroup,
            mobile_user_group = mobileUserGroup,
        };

        return Task.FromResult(response);
    }
}