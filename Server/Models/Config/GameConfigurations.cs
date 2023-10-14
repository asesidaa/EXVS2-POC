using System.ComponentModel.DataAnnotations;

namespace Server.Models.Config;

public class GameConfigurations
{
    public int OfflineBaseWinPoint { get; set; }
    public int OfflineBaseLosePoint { get; set; } 
    public int OfflineLoseResultBonus { get; set; }
    public int CasualBaseWinPoint { get; set; }
    public int CasualBaseLosePoint { get; set; }
    public int CasualLoseResultBonus { get; set; }
    public uint TrainingMinutes { get; set; }
    public uint BattleSeconds { get; set; }

    [Range(1, 8)]
    public uint PvPDamageLevel { get; set; }
}