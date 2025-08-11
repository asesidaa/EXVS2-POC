using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Shared.Dto.Request.Sticker;

public class UpdateDefaultStickerRequest : BasicCardRequest
{
    public StickerDto StickerDto { get; set; } = new();
}