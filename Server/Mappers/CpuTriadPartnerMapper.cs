using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using WebUI.Shared.Dto.Common;

namespace Server.Mappers;

[Mapper]
public static partial class CpuTriadPartnerMapper
{
    [MapProperty(nameof(Response.LoadCard.MobileUserGroup.MstMobileSuitId), nameof(CpuTriadPartner.MobileSuitId))]
    public static partial CpuTriadPartner ToCpuTriadPartner(this Response.LoadCard.MobileUserGroup mobileUserGroup);
}