using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Exception;

namespace Server.Handlers.Card.MobileSuit;

public record GetUsedMobileSuitDataCommand(string AccessCode, string ChipId) : IRequest<List<Response.LoadCard.PilotDataGroup.MSSkillGroup>>;

public class GetUsedMobileSuitDataCommandHandler : IRequestHandler<GetUsedMobileSuitDataCommand, List<Response.LoadCard.PilotDataGroup.MSSkillGroup>>
{
    private readonly ServerDbContext context;
    
    public GetUsedMobileSuitDataCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }
    
    public Task<List<Response.LoadCard.PilotDataGroup.MSSkillGroup>> Handle(GetUsedMobileSuitDataCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = context.CardProfiles
            .Include(x => x.PilotDomain)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var pilotDataGroup = JsonConvert.DeserializeObject<Response.LoadCard.PilotDataGroup>(cardProfile.PilotDomain.PilotDataGroupJson);

        if (pilotDataGroup == null)
        {
            throw new InvalidCardDataException("Card Content is invalid");
        }

        return Task.FromResult(pilotDataGroup.MsSkills);
    }
}