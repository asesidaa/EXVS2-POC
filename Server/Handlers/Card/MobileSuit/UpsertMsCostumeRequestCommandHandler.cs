using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;
using WebUI.Shared.Exception;

namespace Server.Handlers.Card.MobileSuit;

public record UpsertMsCostumeRequestCommand(UpsertMsCostumeRequest Request) : IRequest<BasicResponse>;

public class UpsertMsCostumeRequestCommandHandler : IRequestHandler<UpsertMsCostumeRequestCommand, BasicResponse>
{
    private const uint NonMsId = 0;
    
    private readonly ServerDbContext context;
    
    public UpsertMsCostumeRequestCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<BasicResponse> Handle(UpsertMsCostumeRequestCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;

        if (updateRequest.MobileSuit.MobileSuitId == NonMsId)
        {
            throw new InvalidRequestDataException("Mobile Suit ID is invalid");
        }
        
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
            throw new NullReferenceException("Card Data is invalid");
        }

        var targetMs = pilotDataGroup.MsSkills
            .FirstOrDefault(ms => ms.MstMobileSuitId == updateRequest.MobileSuit.MobileSuitId);

        if (targetMs is null)
        {
            pilotDataGroup.MsSkills.Add(new Response.LoadCard.PilotDataGroup.MSSkillGroup
            {
                MstMobileSuitId = updateRequest.MobileSuit.MobileSuitId,
                CostumeId = updateRequest.MobileSuit.AlternativeCostume ? 1u : 0u,
                MsUsedNum = 0,
                TriadBuddyPoint = 0
            });
        }
        else
        {
            targetMs.CostumeId = updateRequest.MobileSuit.AlternativeCostume ? 1u : 0u;
        }

        cardProfile.PilotDomain.PilotDataGroupJson = JsonConvert.SerializeObject(pilotDataGroup);

        context.SaveChanges();
        
        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
}