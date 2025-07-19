using nue.protocol.exvs;

namespace ServerOver.Mapper.Card.Navi;

using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class NaviMapper
{
    public static partial Response.PreLoadCard.MobileUserGroup.GuestNavGroup ToGuestNaviGroup(this Models.Cards.Navi.Navi navi);
    
    // [MapProperty(nameof(WebUIVanilla.Shared.Dto.Common.Navi.Id), nameof(Models.Cards.Navi.Navi.GuestNavId))]
    // [MapProperty(nameof(WebUIVanilla.Shared.Dto.Common.Navi.CostumeId), nameof(Models.Cards.Navi.Navi.GuestNavCostume))]
    // public static partial Models.Cards.Navi.Navi ToNavi(this WebUIVanilla.Shared.Dto.Common.Navi navi);
}