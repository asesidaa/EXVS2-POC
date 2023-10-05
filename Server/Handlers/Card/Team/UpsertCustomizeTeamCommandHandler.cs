using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Mappers;
using Server.Persistence;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card.Team;

public record UpsertCustomizeTeamCommand(UpsertTeamsRequest Request) : IRequest<BasicResponse>;

public class UpsertCustomizeTeamCommandHandler : IRequestHandler<UpsertCustomizeTeamCommand, BasicResponse>
{
    private readonly ServerDbContext _context;
    
    public UpsertCustomizeTeamCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpsertCustomizeTeamCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;
        
        var cardProfile = _context.CardProfiles
            .Include(x => x.PilotDomain)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var pilotDataGroup = JsonConvert.DeserializeObject<Response.LoadCard.PilotDataGroup>(cardProfile.PilotDomain.PilotDataGroupJson);

        if (pilotDataGroup is null)
        {
            throw new NullReferenceException("User is invalid");
        }

        if (updateRequest.Teams.Count == 0)
        {
            return Task.FromResult(new BasicResponse
            {
                Success = true
            });
        }
        
        pilotDataGroup.TagTeams.Clear();
        
        updateRequest.Teams.ForEach(team =>
        {
            pilotDataGroup.TagTeams.Add(team.ToTagTeamGroup());
        });
        
        cardProfile.PilotDomain.PilotDataGroupJson = JsonConvert.SerializeObject(pilotDataGroup);
        
        _context.SaveChanges();
        
        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
}