using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerOver.Models.Cards.MobileSuit;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Request.Sticker;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Handlers.UI.Sticker;

public record UpsertMobileSuitStickersCommand(UpsertMobileSuitStickersRequest Request) : IRequest<BasicResponse>;

public class UpsertMobileSuitStickersCommandHandler : IRequestHandler<UpsertMobileSuitStickersCommand, BasicResponse>
{
    private readonly ServerDbContext _context;

    public UpsertMobileSuitStickersCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpsertMobileSuitStickersCommand request, CancellationToken cancellationToken)
    {
        var upsertRequest = request.Request;
        
        var response = new BasicResponse
        {
            Success = false
        };

        var cardProfile = _context.CardProfiles
            .Include(x => x.Navis)
            .FirstOrDefault(x => x.AccessCode == upsertRequest.AccessCode && x.ChipId == upsertRequest.ChipId);

        if (cardProfile == null)
        {
            return Task.FromResult(response);
        }
        
        var stickerDtos = upsertRequest.MsStickerList;
        
        stickerDtos.ForEach(stickerDto =>
        {
            var existingSticker = _context.MobileSuitStickerDbSet
                .FirstOrDefault(x => x.CardProfile == cardProfile && x.MstMobileSuitId == stickerDto.MobileSuitId);

            if (existingSticker is null)
            {
                _context.Add(new MobileSuitSticker()
                {
                    CardProfile = cardProfile,
                    MstMobileSuitId = stickerDto.MobileSuitId,
                    PoseId = stickerDto.PoseId,
                    StickerBackgroundId = stickerDto.StickerBackgroundId,
                    StickerEffectId = stickerDto.StickerEffectId,
                    Tracker1 = stickerDto.Tracker1,
                    Tracker2 = stickerDto.Tracker2,
                    Tracker3 = stickerDto.Tracker3,
                });
                
                _context.SaveChanges();
                return;
            }

            existingSticker.PoseId = stickerDto.PoseId;

            existingSticker.StickerBackgroundId = stickerDto.StickerBackgroundId;
            existingSticker.StickerEffectId = stickerDto.StickerEffectId;
            
            existingSticker.Tracker1 = stickerDto.Tracker1;
            existingSticker.Tracker2 = stickerDto.Tracker2;
            existingSticker.Tracker3 = stickerDto.Tracker3;
        });
        
        _context.SaveChanges();

        response.Success = true;

        return Task.FromResult(response);
    }
}