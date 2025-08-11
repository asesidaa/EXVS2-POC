using nue.protocol.exvs;
using ServerOver.Models.Cards.Titles.MobileSuit;

namespace ServerOver.Mapper.Card.Titles.MobileSuit;

using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class MobileSuitTitleMapper
{
    public static partial TitleCustomize ToTitleCustomize(this MobileSuitTitle msTitle);
    
    // [MapProperty(nameof(WebUIVanilla.Shared.Dto.Common.Navi.Id), nameof(Models.Cards.Navi.Navi.GuestNavId))]
    // [MapProperty(nameof(WebUIVanilla.Shared.Dto.Common.Navi.CostumeId), nameof(Models.Cards.Navi.Navi.GuestNavCostume))]
    // public static partial Models.Cards.Navi.Navi ToNavi(this WebUIVanilla.Shared.Dto.Common.Navi navi);
}