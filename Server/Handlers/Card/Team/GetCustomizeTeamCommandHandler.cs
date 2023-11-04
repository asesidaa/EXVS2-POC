using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Mappers;
using Server.Persistence;

namespace Server.Handlers.Card.Team;

public record GetCustomizeTeamCommand(string AccessCode, string ChipId) : IRequest<List<WebUI.Shared.Dto.Common.Team>>;

public class GetCustomizeTeamCommandHandler : IRequestHandler<GetCustomizeTeamCommand, List<WebUI.Shared.Dto.Common.Team>>
{
    private readonly ServerDbContext _context;
    
    public GetCustomizeTeamCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<List<WebUI.Shared.Dto.Common.Team>> Handle(GetCustomizeTeamCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .Include(x => x.PilotDomain)        
            .Include(x => x.TagTeamDataList)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var finalTeamList = new List<WebUI.Shared.Dto.Common.Team>();

        cardProfile.TagTeamDataList
            .ToList()
            .ForEach(team =>
            {
                var partner = _context.CardProfiles
                    .Include(x => x.UserDomain)
                    .FirstOrDefault(x => x.Id == (int)team.TeammateCardId);

                if (partner is null)
                {
                    return;
                }
                
                var partnerMobileUserGroup =
                    JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(partner.UserDomain.UserJson);

                if (partnerMobileUserGroup is null)
                {
                    return;
                }

                var teamDto = team.ToTeam();
                teamDto.PartnerId = team.TeammateCardId;
                teamDto.PartnerName = partnerMobileUserGroup.PlayerName;

                finalTeamList.Add(teamDto);
            });

        var oppositeTeams = _context.TagTeamData
            .Where(team => team.TeammateCardId == cardProfile.Id)
            .ToList();
        
        oppositeTeams.ForEach(team =>
        {
            var partner = _context.CardProfiles
                .Include(x => x.UserDomain)
                .FirstOrDefault(x => x.Id == team.CardId);

            if (partner is null)
            {
                return;
            }
            
            var partnerMobileUserGroup = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(partner.UserDomain.UserJson);

            if (partnerMobileUserGroup is null)
            {
                return;
            }

            var teamDto = team.ToTeam();
            teamDto.PartnerId = (uint) team.CardId;
            teamDto.PartnerName = partnerMobileUserGroup.PlayerName;
            
            finalTeamList.Add(teamDto);
        });
        
        return Task.FromResult(finalTeamList);
    }
}