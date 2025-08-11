using MediatR;
using Microsoft.EntityFrameworkCore;
using nue.protocol.exvs;
using ServerOver.Mapper.Card.Titles.User;
using ServerOver.Persistence;
using ServerOver.Strategy.Team;

namespace ServerOver.Handlers.Game.Tournament;

public record LoadMeetingCardCommand(Request Request) : IRequest<Response>;

public class LoadMeetingCardCommandHandler : IRequestHandler<LoadMeetingCardCommand, Response>
{
    private readonly ILogger<LoadMeetingCardCommandHandler> _logger;
    private readonly ServerDbContext _context;

    public LoadMeetingCardCommandHandler(ILogger<LoadMeetingCardCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task<Response> Handle(LoadMeetingCardCommand query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
        };
        
        var loadMeetingCardRequest = request.load_meeting_card;

        var accessCode = loadMeetingCardRequest.AccessCode;
        var chipId = loadMeetingCardRequest.ChipId;

        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == accessCode && x.ChipId == chipId);

        if (cardProfile is null)
        {
            response.Error = Error.ErrServer;
            return Task.FromResult(response);
        }

        var playerLevel = _context.PlayerLevelDbSet
            .First(x => x.CardProfile == cardProfile);

        var winLossRecord = _context.WinLossRecordDbSet
            .First(x => x.CardProfile == cardProfile);

        var playerProfile = _context.PlayerProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        var titleSetting = _context.UserTitleSettingDbSet
            .Include(x => x.UserDefaultTitle)
            .First(x => x.CardProfile == cardProfile);

        response.load_meeting_card = new Response.LoadMeetingCard()
        {
            IsNewCard = false,
            LoadPlayer = new LoadPlayer()
            {
                LastPlayedAt = cardProfile.LastPlayedAt,
                PilotId = (uint)cardProfile.Id,
                PlayerLevelId = playerLevel.PlayerLevelId,
                PrestigeId = playerLevel.PrestigeId,
                TotalWin = winLossRecord.TotalWin,
                TotalLose = winLossRecord.TotalLose,
            },
            AccessCode = accessCode,
            User = new Response.LoadMeetingCard.MobileUserGroup()
            {
                UserId = (uint)cardProfile.Id,
                PlayerName = cardProfile.UserName,
                OpenEchelon = playerProfile.OpenEchelon,
                OpenRecord = playerProfile.OpenRecord,
                DefaultTitleCustomize = titleSetting.UserDefaultTitle.ToTitleCustomize()
            }
        };
        
        var playerLeaderTeamAppendStrategy = new PlayerLeaderTeamAppendStrategy(_context);
        var playerPartnerTeamAppendStrategy = new PlayerPartnerTeamAppendStrategy(_context);
        
        playerLeaderTeamAppendStrategy.Append(cardProfile, response.load_meeting_card.LoadPlayer.TagTeams);
        playerPartnerTeamAppendStrategy.Append(cardProfile, response.load_meeting_card.LoadPlayer.TagTeams);
        
        return Task.FromResult(response);
    }
}