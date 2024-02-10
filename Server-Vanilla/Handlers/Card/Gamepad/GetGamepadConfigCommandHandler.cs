using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Mapper.Card.Setting;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Common;

namespace ServerVanilla.Handlers.Card.Gamepad;

public record GetGamepadConfigCommand(string AccessCode, string ChipId) : IRequest<GamepadConfig>;

public class GetGamepadConfigCommandHandler : IRequestHandler<GetGamepadConfigCommand, GamepadConfig>
{
    private readonly ServerDbContext _context;
    
    public GetGamepadConfigCommandHandler(ServerDbContext context)
    {
        _context = context;
    }
    
    public Task<GamepadConfig> Handle(GetGamepadConfigCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .Include(x => x.GamepadSetting)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        return Task.FromResult(cardProfile.GamepadSetting.ToGamepadConfig());
    }
}