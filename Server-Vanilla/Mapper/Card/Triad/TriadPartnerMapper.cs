using ServerVanilla.Models.Cards.Triad;
using WebUIVanilla.Shared.Dto.Common;

namespace ServerVanilla.Mapper.Card.Triad;

using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class TriadPartnerMapper
{
    [MapProperty(nameof(TriadPartner.MstMobileSuitId), nameof(CpuTriadPartner.MobileSuitId))]
    [MapProperty(nameof(TriadPartner.MsSkill1), nameof(CpuTriadPartner.Skill1))]
    [MapProperty(nameof(TriadPartner.MsSkill2), nameof(CpuTriadPartner.Skill2))]
    public static partial CpuTriadPartner ToCpuTriadPartner(this TriadPartner triadPartner);
}