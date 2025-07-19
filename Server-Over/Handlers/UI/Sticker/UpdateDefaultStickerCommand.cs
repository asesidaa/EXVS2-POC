using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Request.Sticker;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Handlers.UI.Sticker;

public record UpdateDefaultStickerCommand(UpdateDefaultStickerRequest Request) : IRequest<BasicResponse>;

public class UpdateDefaultStickerCommandHandler : IRequestHandler<UpdateDefaultStickerCommand, BasicResponse>
{
    private readonly ServerDbContext _context;

    public UpdateDefaultStickerCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpdateDefaultStickerCommand request, CancellationToken cancellationToken)
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

        var defaultStickerProfile = _context.DefaultStickerProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        var stickerDto = upsertRequest.StickerDto;

        defaultStickerProfile.BasePanelId = stickerDto.BasePanelId;
        defaultStickerProfile.CommentPartsAId = stickerDto.CommentPartsAId;
        defaultStickerProfile.CommentPartsBId = stickerDto.CommentPartsBId;
        
        defaultStickerProfile.StickerBackgroundId = stickerDto.StickerBackgroundId;
        defaultStickerProfile.StickerEffectId = stickerDto.StickerEffectId;
        
        defaultStickerProfile.Tracker1 = stickerDto.Tracker1;
        defaultStickerProfile.Tracker2 = stickerDto.Tracker2;
        defaultStickerProfile.Tracker3 = stickerDto.Tracker3;
        
        _context.SaveChanges();

        response.Success = true;

        return Task.FromResult(response);
    }
}