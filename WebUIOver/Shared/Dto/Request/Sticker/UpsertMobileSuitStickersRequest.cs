using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Shared.Dto.Request.Sticker;

public class UpsertMobileSuitStickersRequest : BasicCardRequest
{
    public List<StickerDto> MsStickerList { get; set; } = new();
}