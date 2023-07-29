using WebUI.Shared.Dto.Common;

namespace WebUI.Shared.Dto.Request;

public class UpdateCpuTriadPartnerRequest : BasicCardRequest
{
    public CpuTriadPartner CpuTriadPartner { get; set; } = new();
}