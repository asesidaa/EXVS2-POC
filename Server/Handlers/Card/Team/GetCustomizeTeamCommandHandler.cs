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
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var pilotDataGroup = JsonConvert.DeserializeObject<Response.LoadCard.PilotDataGroup>(cardProfile.PilotDomain.PilotDataGroupJson);

        if (pilotDataGroup is null)
        {
            throw new NullReferenceException("User is invalid");
        }

        var teams = pilotDataGroup.TagTeams
            .Select(tagTeam => tagTeam.ToTeam())
            .ToList();
        
        return Task.FromResult(teams);
    }
}