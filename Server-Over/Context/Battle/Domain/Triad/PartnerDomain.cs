namespace ServerOver.Context.Battle.Domain.Triad;

public class PartnerDomain
{
    public bool IsTriadPartner { get; set; } = true;
    public uint TriadPartnerMsId { get; set; } = 0;
}