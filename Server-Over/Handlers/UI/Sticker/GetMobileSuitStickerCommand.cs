using MediatR;
using ServerOver.Mapper.Card.Sticker;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.Sticker;

public record GetMobileSuitStickerCommand(string AccessCode, string ChipId) : IRequest<List<StickerDto>>;
public class GetMobileSuitStickerCommandHandler : IRequestHandler<GetMobileSuitStickerCommand, List<StickerDto>>
{
    private readonly ServerDbContext _context;

    public GetMobileSuitStickerCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<List<StickerDto>> Handle(GetMobileSuitStickerCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile == null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        var mobileSuitStickers = _context.MobileSuitStickerDbSet
            .Where(x => x.CardProfile == cardProfile)
            .OrderBy(x => x.MstMobileSuitId)
            .Select(sticker => sticker.ToMobileSuitStickerDto())
            .ToList();
        
        return Task.FromResult(mobileSuitStickers);
    }
}