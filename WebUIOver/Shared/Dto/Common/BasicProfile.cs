﻿using WebUIOver.Shared.Dto.Enum;

namespace WebUIOver.Shared.Dto.Common;

public class BasicProfile
{
    public uint UserId { get; set; } = 0;
    public string UserName { get; set; } = string.Empty;
    public uint OpenRecord { get; set; } = 0;
    public uint OpenEchelon { get; set; } = 0;
    public uint DefaultGaugeDesignId { get; set; } = 0;
    public BgmPlayingMethod DefaultBgmPlayingMethod { get; set; } = BgmPlayingMethod.Random;
    public uint[] DefaultBgmList { get; set; } = Array.Empty<uint>();
    public uint[] StageRandoms { get; set; } = Array.Empty<uint>();
    public Title DefaultTitle { get; set; } = new();
    public Title TriadTitle { get; set; } = new();
    public Title ClassMatchTitle { get; set; } = new();
    public bool IsFixedRadar { get; set; } = false;
    public uint Gp { get; set; } = 0;
}