using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Mappers;
using Server.Persistence;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;
using WebUI.Shared.Exception;

namespace Server.Handlers.Card.Gamepad;

public record UpsertGamepadConfigCommand(UpsertGamepadConfigRequest Request) : IRequest<BasicResponse>;

public class UpsertGamepadConfigCommandHandler : IRequestHandler<UpsertGamepadConfigCommand, BasicResponse>
{
    private readonly ServerDbContext context;
    
    public UpsertGamepadConfigCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }
    
    public Task<BasicResponse> Handle(UpsertGamepadConfigCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;
        
        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);

        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var mobileUserGroup = JsonConvert.DeserializeObject<Response.LoadCard.MobileUserGroup>(cardProfile.UserDomain.MobileUserGroupJson);
        
        if (mobileUserGroup is null)
        {
            throw new InvalidCardDataException("Card Data is invalid");
        }

        mobileUserGroup.Gamepad = updateRequest.GamepadConfig.ToGamepadGroup();
        
        cardProfile.UserDomain.MobileUserGroupJson = JsonConvert.SerializeObject(mobileUserGroup);
        
        context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
}