using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerOver.Models.Cards.MobileSuit;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.MobileSuit;

public record UpdateAllMsCostumeRequestCommand(UpdateAllMsCostumeSkinRequest Request) : IRequest<BasicResponse>;

public class UpdateAllMsCostumeRequestCommandHandler : IRequestHandler<UpdateAllMsCostumeRequestCommand, BasicResponse>
{
    private readonly ServerDbContext _context;
    
    public UpdateAllMsCostumeRequestCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpdateAllMsCostumeRequestCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;

        var cardProfile = _context.CardProfiles
            .Include(x => x.MobileSuits)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);
        
        if (cardProfile == null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }
        
        updateRequest.MsSkillGroup.ForEach(msSkillGroup => UpsertMsSkill(msSkillGroup, cardProfile.MobileSuits));
        
        _context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }

    void UpsertMsSkill(MsSkillGroup msSkill, ICollection<MobileSuitUsage> mobileSuitUsages)
    {
        var existingMsSkill = mobileSuitUsages.
            FirstOrDefault(pilotMsSkill => pilotMsSkill.MstMobileSuitId == msSkill.MstMobileSuitId);

        if (existingMsSkill is null)
        {
            mobileSuitUsages.Add(new MobileSuitUsage()
            {
                MstMobileSuitId = msSkill.MstMobileSuitId,
                CostumeId = msSkill.CostumeId,
                MsUsedNum = 0
            });
        }
        else
        {
            existingMsSkill.CostumeId = msSkill.CostumeId;
        }
    }
}