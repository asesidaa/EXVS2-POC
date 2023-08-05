using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using WebUI.Shared.Dto.Common;

namespace Server.Mappers;

[Mapper]
public static partial class NaviMapper
{
    [MapProperty(nameof(Navi.Id), nameof(Response.PreLoadCard.MobileUserGroup.GuestNavGroup.GuestNavId))]
    [MapProperty(nameof(Navi.CostumeId), nameof(Response.PreLoadCard.MobileUserGroup.GuestNavGroup.GuestNavCostume))]
    public static partial Response.PreLoadCard.MobileUserGroup.GuestNavGroup ToGuestNaviGroup(this Navi navi);
}