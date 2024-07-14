using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Mapper.Card;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Common;

namespace ServerVanilla.Handlers.Card.Profile;

public record GetEchelonProfileCommand(string AccessCode, string ChipId) : IRequest<EchelonProfile>;

public class GetEchelonProfileCommandHandler : IRequestHandler<GetEchelonProfileCommand, EchelonProfile>
{
    private readonly ServerDbContext _context;

    public GetEchelonProfileCommandHandler(ServerDbContext context)
    {
        _context = context;
    }
    
    public Task<EchelonProfile> Handle(GetEchelonProfileCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .Include(x => x.BattleProfile)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var battleProfile = cardProfile.BattleProfile;
        
        if (battleProfile is null)
        {
            throw new NullReferenceException("User is invalid");
        }
        
        return Task.FromResult(battleProfile.ToEchelonProfile());
    }
}