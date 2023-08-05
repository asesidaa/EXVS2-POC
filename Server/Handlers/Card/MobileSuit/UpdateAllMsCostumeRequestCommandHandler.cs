using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Mappers;
using Server.Persistence;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;
using WebUI.Shared.Exception;

namespace Server.Handlers.Card.MobileSuit;

public record UpdateAllMsCostumeRequestCommand(UpdateAllMsCostumeRequest Request) : IRequest<BasicResponse>;

public class UpdateAllMsCostumeRequestCommandHandler : IRequestHandler<UpdateAllMsCostumeRequestCommand, BasicResponse>
{
    private readonly ServerDbContext context;
    
    public UpdateAllMsCostumeRequestCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<BasicResponse> Handle(UpdateAllMsCostumeRequestCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;

        var cardProfile = context.CardProfiles
            .Include(x => x.PilotDomain)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var pilotDataGroup = JsonConvert.DeserializeObject<Response.LoadCard.PilotDataGroup>(cardProfile.PilotDomain.PilotDataGroupJson);

        if (pilotDataGroup == null)
        {
            throw new InvalidCardDataException("Card Content is invalid");
        }

        // reverse map
        var msSkills = updateRequest.MsSkillGroup.Select(x => x.ToMsSkillGroupMapper()).ToList();
        
        msSkills.ForEach(UpsertMsSkill(pilotDataGroup));

        cardProfile.PilotDomain.PilotDataGroupJson = JsonConvert.SerializeObject(pilotDataGroup);
        
        context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }

    Action<Response.LoadCard.PilotDataGroup.MSSkillGroup> UpsertMsSkill(Response.LoadCard.PilotDataGroup pilotDataGroup)
    {
        return msSkill =>
        {
            var existingMsSkill = pilotDataGroup.MsSkills.FirstOrDefault(pilotMsSkill => pilotMsSkill.MstMobileSuitId == msSkill.MstMobileSuitId);

            if (existingMsSkill is null)
            {
                pilotDataGroup.MsSkills.Add(msSkill);
                return;
            }

            existingMsSkill.CostumeId = msSkill.CostumeId;
        };
    }
}