using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Mapper.Card;
using ServerVanilla.Persistence;

namespace ServerVanilla.Handlers.Card.Team;

public record GetCustomizeTeamCommand(string AccessCode, string ChipId) : IRequest<List<WebUIVanilla.Shared.Dto.Common.Team>>;

public class GetCustomizeTeamCommandHandler : IRequestHandler<GetCustomizeTeamCommand, List<WebUIVanilla.Shared.Dto.Common.Team>>
{
    private readonly ServerDbContext _context;
    
    public GetCustomizeTeamCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<List<WebUIVanilla.Shared.Dto.Common.Team>> Handle(GetCustomizeTeamCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .Include(x => x.TagTeamDatas)
            .Include(x => x.OnlinePairs)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var finalTeamList = new List<WebUIVanilla.Shared.Dto.Common.Team>();

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

        var oppositeTeams = _context.TagTeamData
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
        
        return Task.FromResult(finalTeamList);
    }
}