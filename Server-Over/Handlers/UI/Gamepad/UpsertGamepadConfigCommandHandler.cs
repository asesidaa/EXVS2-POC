using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.Gamepad;

public record UpsertGamepadConfigCommand(UpsertGamepadConfigRequest Request) : IRequest<BasicResponse>;

public class UpsertGamepadConfigCommandHandler : IRequestHandler<UpsertGamepadConfigCommand, BasicResponse>
{
    private readonly ServerDbContext _context;
    
    public UpsertGamepadConfigCommandHandler(ServerDbContext context)
    {
        _context = context;
    }
    
    public Task<BasicResponse> Handle(UpsertGamepadConfigCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;
        
        var cardProfile = _context.CardProfiles
            .Include(x => x.GamepadSetting)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);

        if (cardProfile is null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        cardProfile.GamepadSetting.AKey = updateRequest.GamepadConfig.AKey;
        cardProfile.GamepadSetting.BKey = updateRequest.GamepadConfig.BKey;
        cardProfile.GamepadSetting.XKey = updateRequest.GamepadConfig.XKey;
        cardProfile.GamepadSetting.YKey = updateRequest.GamepadConfig.YKey;
        cardProfile.GamepadSetting.LbKey = updateRequest.GamepadConfig.LbKey;
        cardProfile.GamepadSetting.RbKey = updateRequest.GamepadConfig.RbKey;
        cardProfile.GamepadSetting.LtKey = updateRequest.GamepadConfig.LtKey;
        cardProfile.GamepadSetting.RtKey = updateRequest.GamepadConfig.RtKey;
        cardProfile.GamepadSetting.LsbKey = updateRequest.GamepadConfig.LsbKey;
        cardProfile.GamepadSetting.RsbKey = updateRequest.GamepadConfig.RsbKey;
        
        _context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
}