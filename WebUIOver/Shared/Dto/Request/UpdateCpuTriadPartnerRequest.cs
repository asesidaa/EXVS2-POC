using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Shared.Dto.Request;

public class UpdateCpuTriadPartnerRequest : BasicCardRequest
{
    public CpuTriadPartner CpuTriadPartner { get; set; } = new();
}