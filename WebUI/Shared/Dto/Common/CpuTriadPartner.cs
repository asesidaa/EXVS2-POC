using WebUI.Shared.Dto.Enum;

namespace WebUI.Shared.Dto.Common;

public class CpuTriadPartner
{
    public uint MobileSuitId { get; set; } = 0;
    public uint ArmorLevel { get; set; } = 0;
    public uint ShootAttackLevel { get; set; } = 0;
    public uint InfightAttackLevel { get; set; } = 0;
    public uint BoosterLevel { get; set; } = 0;
    public uint ExGaugeLevel { get; set; } = 0;
    public uint AiLevel { get; set; } = 0;
    public uint Skill1 { get; set; } = 0;
    public uint Skill2 { get; set; } = 0;
    public BurstType BurstType { get; set; } = BurstType.Covering;
    public string TriadTeamName { get; set; } = string.Empty;
    public uint TriadBackgroundPartsId { get; set; } = 0;
}