using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.Triad;
using WebUIOver.Shared.Dto.Common;

namespace ServerOver.Mapper.Card.Triad;

[Mapper]
public static partial class CpuTriadPartnerMapper
{
    [MapProperty(nameof(TriadPartner.MstMobileSuitId), nameof(CpuTriadPartner.MobileSuitId))]
    [MapProperty(nameof(TriadPartner.MsSkill1), nameof(CpuTriadPartner.Skill1))]
    [MapProperty(nameof(TriadPartner.MsSkill2), nameof(CpuTriadPartner.Skill2))]
    public static partial CpuTriadPartner ToCpuTriadPartner(this TriadPartner triadPartner);
}