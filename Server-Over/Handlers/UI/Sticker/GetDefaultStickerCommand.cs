using MediatR;
using ServerOver.Mapper.Card.Sticker;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.Sticker;

public record GetDefaultStickerCommand(string AccessCode, string ChipId) : IRequest<StickerDto>;
public class GetDefaultStickerCommandHandler : IRequestHandler<GetDefaultStickerCommand, StickerDto>
{
    private readonly ServerDbContext _context;

    public GetDefaultStickerCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<StickerDto> Handle(GetDefaultStickerCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile == null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        var defaultStickerProfile = _context.DefaultStickerProfileDbSet
            .First(x => x.CardProfile == cardProfile);
        
        return Task.FromResult(defaultStickerProfile.ToPlayerStickerDto());
    }
}