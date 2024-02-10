using WebUIVanilla.Shared.Dto.Common;

namespace WebUIVanilla.Shared.Dto.Request;

public class UpdateCpuTriadPartnerRequest : BasicCardRequest
{
    public CpuTriadPartner CpuTriadPartner { get; set; } = new();
}