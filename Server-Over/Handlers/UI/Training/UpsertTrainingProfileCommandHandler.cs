using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Dto.Training;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.Training;

public record UpsertTrainingProfileCommand(UpdateTrainingProfileRequest Request) : IRequest<BasicResponse>;

public class UpsertGamepadConfigCommandHandler : IRequestHandler<UpsertTrainingProfileCommand, BasicResponse>
{
    private readonly ServerDbContext _context;
    
    public UpsertGamepadConfigCommandHandler(ServerDbContext context)
    {
        _context = context;
    }
    
    public Task<BasicResponse> Handle(UpsertTrainingProfileCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;
        
        var cardProfile = _context.CardProfiles
            .Include(x => x.TrainingProfile)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);

        if (cardProfile is null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        cardProfile.TrainingProfile.MstMobileSuitId = updateRequest.TrainingProfile.MstMobileSuitId;
        cardProfile.TrainingProfile.BurstType = (uint) updateRequest.TrainingProfile.BurstType;
        cardProfile.TrainingProfile.CpuLevel = updateRequest.TrainingProfile.CpuLevel;
        cardProfile.TrainingProfile.ExBurstGauge = updateRequest.TrainingProfile.ExBurstGauge;
        cardProfile.TrainingProfile.DamageDisplay = updateRequest.TrainingProfile.DamageDisplay;
        cardProfile.TrainingProfile.CpuAutoGuard = updateRequest.TrainingProfile.CpuAutoGuard;
        cardProfile.TrainingProfile.CommandGuideDisplay = updateRequest.TrainingProfile.CommandGuideDisplay;
        
        _context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
}