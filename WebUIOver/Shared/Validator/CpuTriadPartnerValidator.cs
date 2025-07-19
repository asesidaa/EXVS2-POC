using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Shared.Validator;

public class CpuTriadPartnerValidator
{
    private const uint MaxIndividualLv = 100;
    private const uint MaxAggregatedLv = 500;
    public bool Validate(CpuTriadPartner cpuTriadPartner)
    {
        int totalLevel = 0;
        
        if (cpuTriadPartner.ArmorLevel > MaxIndividualLv)
        {
            return false;
        }

        totalLevel += cpuTriadPartner.ArmorLevel;
        
        if (cpuTriadPartner.ShootAttackLevel > MaxIndividualLv)
        {
            return false;
        }
        
        totalLevel += cpuTriadPartner.ShootAttackLevel;
        
        if (cpuTriadPartner.InfightAttackLevel > MaxIndividualLv)
        {
            return false;
        }
        
        totalLevel += cpuTriadPartner.InfightAttackLevel;
        
        if (cpuTriadPartner.BoosterLevel > MaxIndividualLv)
        {
            return false;
        }
        
        totalLevel += cpuTriadPartner.BoosterLevel;
        
        if (cpuTriadPartner.ExGaugeLevel > MaxIndividualLv)
        {
            return false;
        }
        
        totalLevel += cpuTriadPartner.ExGaugeLevel;
        
        if (cpuTriadPartner.AiLevel > MaxIndividualLv)
        {
            return false;
        }
        
        totalLevel += cpuTriadPartner.AiLevel;
        
        return (totalLevel <= MaxAggregatedLv);
    }
}