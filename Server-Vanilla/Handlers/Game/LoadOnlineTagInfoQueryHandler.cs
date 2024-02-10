using MediatR;
using Microsoft.EntityFrameworkCore;
using nue.protocol.exvs;
using ServerVanilla.Models.Cards;
using ServerVanilla.Models.Cards.Team;
using ServerVanilla.Persistence;

namespace ServerVanilla.Handlers.Game;

public record LoadOnlineTagInfoQuery(Request Request) : IRequest<Response>;

public class LoadOnlineTagInfoQueryHandler : IRequestHandler<LoadOnlineTagInfoQuery, Response>
{
    private readonly ILogger<PreLoadCardQueryHandler> _logger;
    private readonly ServerDbContext _context;

    public LoadOnlineTagInfoQueryHandler(ILogger<PreLoadCardQueryHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task<Response> Handle(LoadOnlineTagInfoQuery request, CancellationToken cancellationToken)
    {
        var commandRequest = request.Request;
        var loadTagInfoRequest = request.Request.load_tag_info;

        var overallResponse = new Response
        {
            Type = commandRequest.Type,
            RequestId = commandRequest.RequestId,
            Error = Error.Success
        };
        
        var cardProfile = _context.CardProfiles
            .Include(x => x.OnlinePairs)
            .FirstOrDefault(card => card.Id == loadTagInfoRequest.PilotId);
        
        if (cardProfile == null)
        {
            overallResponse.Error = Error.ErrServer;
            return Task.FromResult(overallResponse);
        }

        var loadTagInfoResponse = new Response.LoadTagInfo();

        cardProfile.OnlinePairs
            .Where(x => x.CardId != 0 && x.PairId != 0)
            .ToList()
            .ForEach(onlinePair => { AddTagTeamPartner(onlinePair, cardProfile, loadTagInfoResponse); });

        return Task.FromResult(new Response
        {
            Type = commandRequest.Type,
            RequestId = commandRequest.RequestId,
            Error = Error.Success,
            load_tag_info = loadTagInfoResponse
        });
    }

    void AddTagTeamPartner(OnlinePair onlinePair, CardProfile cardProfile, Response.LoadTagInfo loadTagInfoResponse)
    {
        var team = _context.TagTeamData.FirstOrDefault(tagTeam => tagTeam.Id == onlinePair.TeamId);

        if (team is null)
        {
            return;
        }

        var partnerId = 0;

        if (team.CardId != cardProfile.Id)
        {
            partnerId = team.CardId;
        }

        if (team.TeammateCardId != cardProfile.Id)
        {
            partnerId = (int)team.TeammateCardId;
        }

        var partnerProfile = _context.CardProfiles
            .Include(x => x.BattleProfile)
            .Include(x => x.DefaultTitle)
            .FirstOrDefault(x => x.Id == partnerId);

        if (partnerProfile is null)
        {
            return;
        }

        loadTagInfoResponse.TagTeamPartners.Add(new Response.LoadTagInfo.TagTeamPartner()
        {
            Id = (uint)team.Id,
            Name = team.TeamName,
            PartnerId = (uint)partnerProfile.Id,
            EchelonId = partnerProfile.BattleProfile.EchelonId,
            SEchelonFlag = partnerProfile.BattleProfile.SEchelonFlag,
            TitleTextId = partnerProfile.DefaultTitle.TitleTextId,
            TitleOrnamentId = partnerProfile.DefaultTitle.TitleOrnamentId,
            TitleEffectId = partnerProfile.DefaultTitle.TitleEffectId,
            TitleBackgroundPartsId = partnerProfile.DefaultTitle.TitleBackgroundPartsId,
            PartnerName = partnerProfile.UserName
        });
    }
}