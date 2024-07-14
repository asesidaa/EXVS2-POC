using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Request;
using WebUIVanilla.Shared.Dto.Response;
using WebUIVanilla.Shared.Exception;

namespace ServerVanilla.Handlers.Card.Echelon;

public record UpdateEchelonTestSettingCommand(UpdateEchelonTestSettingRequest Request) : IRequest<BasicResponse>;

public class UpdateEchelonTestSettingCommandHandler : IRequestHandler<UpdateEchelonTestSettingCommand, BasicResponse>
{
    private readonly ServerDbContext _context;
    
    public UpdateEchelonTestSettingCommandHandler(ServerDbContext context)
    {
        _context = context;
    }
    
    public Task<BasicResponse> Handle(UpdateEchelonTestSettingCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;
        
        var cardProfile = _context.CardProfiles
            .Include(x => x.BattleProfile)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);

        if (cardProfile is null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        var battleProfile = cardProfile.BattleProfile;

        if (battleProfile is null)
        {
            throw new InvalidCardDataException("Card Data is invalid");
        }

        if (battleProfile.EchelonId != 23 && battleProfile.EchelonId != 38)
        {
            return Task.FromResult(new BasicResponse
            {
                Success = true
            });
        }
        
        battleProfile.SEchelonMissionFlag = updateRequest.ParticipateInTest;
        battleProfile.SEchelonFlag = false;
        battleProfile.SEchelonProgress = 0;

        switch (battleProfile.EchelonId)
        {
            case 23:
                battleProfile.SCaptainFlag = false;
                break;
            case 38:
                battleProfile.SBrigadierFlag = false;
                break;
        }
        
        _context.SaveChanges();
        
        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
}