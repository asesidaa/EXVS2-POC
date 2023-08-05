using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Mappers;
using Server.Persistence;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Exception;

namespace Server.Handlers.Card.Gamepad;

public record GetGamepadConfigCommand(string AccessCode, string ChipId) : IRequest<GamepadConfig>;

public class GetGamepadConfigCommandHandler : IRequestHandler<GetGamepadConfigCommand, GamepadConfig>
{
    private readonly ServerDbContext context;
    
    public GetGamepadConfigCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }
    
    public Task<GamepadConfig> Handle(GetGamepadConfigCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var mobileUserGroup = JsonConvert.DeserializeObject<Response.LoadCard.MobileUserGroup>(cardProfile.UserDomain.MobileUserGroupJson);
        
        if (mobileUserGroup is null)
        {
            throw new InvalidCardDataException("Card Data is invalid");
        }

        if (mobileUserGroup.Gamepad is null)
        {
            return Task.FromResult(new GamepadConfig());
        }
        
        return Task.FromResult(mobileUserGroup.Gamepad.ToGamePadConfig());
    }
}