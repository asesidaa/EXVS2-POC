using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;
using WebUI.Shared.Exception;

namespace Server.Handlers.Card.Echelon;

public record UpdateEchelonTestSettingCommand(UpdateEchelonTestSettingRequest Request) : IRequest<BasicResponse>;

public class UpdateEchelonTestSettingCommandHandler : IRequestHandler<UpdateEchelonTestSettingCommand, BasicResponse>
{
    private readonly ServerDbContext context;
    
    public UpdateEchelonTestSettingCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<BasicResponse> Handle(UpdateEchelonTestSettingCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;
        
        var cardProfile = context.CardProfiles
            .Include(x => x.PilotDomain)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);

        if (cardProfile is null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }
        
        var loadPlayer = JsonConvert.DeserializeObject<Response.PreLoadCard.LoadPlayer>(cardProfile.PilotDomain.LoadPlayerJson);
        
        if (loadPlayer is null)
        {
            throw new InvalidCardDataException("Card Data is invalid");
        }

        if (loadPlayer.EchelonId != 23 && loadPlayer.EchelonId != 38)
        {
            return Task.FromResult(new BasicResponse
            {
                Success = true
            });
        }
        
        loadPlayer.SEchelonMissionFlag = updateRequest.ParticipateInTest;
        loadPlayer.SEchelonFlag = false;
        loadPlayer.SEchelonProgress = 0;

        switch (loadPlayer.EchelonId)
        {
            case 23:
                loadPlayer.SCaptainFlag = false;
                break;
            case 38:
                loadPlayer.SBrigadierFlag = false;
                break;
        }

        context.SaveChanges();
        
        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
}