using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.MobileSuit;
using ServerOver.Models.Cards.Profile;
using WebUIOver.Shared.Dto.Common;

namespace ServerOver.Mapper.Card.Sticker;

[Mapper]
public static partial class StickerMapper
{
    public static partial StickerDto ToPlayerStickerDto(this DefaultStickerProfile defaultStickerProfile);
    
    [MapProperty(nameof(MobileSuitSticker.MstMobileSuitId), nameof(StickerDto.MobileSuitId))]
    public static partial StickerDto ToMobileSuitStickerDto(this MobileSuitSticker mobileSuitSticker);
}