﻿using WebUIOver.Shared.Dto.Enum;

namespace WebUIOver.Shared.Dto.Common;

public class CpuTriadPartner
{
    public uint MobileSuitId { get; set; } = 0;
    public int ArmorLevel { get; set; } = 0;
    public int ShootAttackLevel { get; set; } = 0;
    public int InfightAttackLevel { get; set; } = 0;
    public int BoosterLevel { get; set; } = 0;
    public int ExGaugeLevel { get; set; } = 0;
    public int AiLevel { get; set; } = 0;
    public uint Skill1 { get; set; } = 0;
    public uint Skill2 { get; set; } = 0;
    public BurstType BurstType { get; set; } = BurstType.Covering;
    public string TriadTeamName { get; set; } = string.Empty;
    public uint TriadBackgroundPartsId { get; set; } = 0;
}