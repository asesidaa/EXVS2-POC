using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerOver.Mapper.Card.Team;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.Team;

public record GetCustomizeTeamCommand(string AccessCode, string ChipId) : IRequest<TeamResponse>;

public class GetCustomizeTeamCommandHandler : IRequestHandler<GetCustomizeTeamCommand, TeamResponse>
{
    private readonly ServerDbContext _context;
    
    public GetCustomizeTeamCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<TeamResponse> Handle(GetCustomizeTeamCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .Include(x => x.TagTeamDatas)
            .Include(x => x.OnlinePairs)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);
        
        if (cardProfile == null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        var finalTeamList = new List<WebUIOver.Shared.Dto.Common.Team>();

        cardProfile.TagTeamDatas
            .ToList()
            .ForEach(team =>
            {
                var partner = _context.CardProfiles
                    .FirstOrDefault(x => x.Id == (int)team.TeammateCardId);

                if (partner is null)
                {
                    return;
                }
                
                var teamDto = team.ToTeam();
                teamDto.PartnerId = team.TeammateCardId;
                teamDto.PartnerName = partner.UserName;

                var onlineTag = cardProfile.OnlinePairs
                    .FirstOrDefault(dbOnlinePair => dbOnlinePair.TeamId == team.Id);

                if (onlineTag is not null)
                {
                    teamDto.OnlineTag = true;
                }
                
                finalTeamList.Add(teamDto);
            });

        var oppositeTeams = _context.TagTeamDataDbSet
            .Where(team => team.TeammateCardId == cardProfile.Id)
            .ToList();
        
        oppositeTeams.ForEach(team =>
        {
            var partner = _context.CardProfiles
                .FirstOrDefault(x => x.Id == team.CardId);

            if (partner is null)
            {
                return;
            }
            
            var teamDto = team.ToTeam();
            teamDto.PartnerId = (uint) team.CardId;
            teamDto.PartnerName = partner.UserName;
            
            var onlineTag = cardProfile.OnlinePairs
                .FirstOrDefault(dbOnlinePair => dbOnlinePair.TeamId == team.Id);

            if (onlineTag is not null)
            {
                teamDto.OnlineTag = true;
            }
            
            finalTeamList.Add(teamDto);
        });
        
        return Task.FromResult(new TeamResponse()
        {
            DistinctTeamFormationToken = cardProfile.DistinctTeamFormationToken,
            Teams = finalTeamList
        });
    }
}