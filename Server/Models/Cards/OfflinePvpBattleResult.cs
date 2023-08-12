using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.Cards;

[Table("offline_pvp_battle_result")]
public class OfflinePvpBattleResult : BaseBattleResult
{
    [Required]
    public String OfflineBattleMode { get; set; } = string.Empty;

    public bool SEchelonFlag { get; set; } = false;

    public uint SEchelonProgress { get; set; } = 0;
    public uint PartnerPilotId { get; set; } = 0;
    public uint PartnerMsId { get; set; } = 0;
    public uint PartnerEchelonId { get; set; } = 0;
    public uint PartnerBurstType { get; set; } = 0;
    public uint Foe1PilotId { get; set; } = 0;
    public uint Foe1MsId { get; set; } = 0;
    public uint Foe1EchelonId { get; set; } = 0;
    public uint Foe1BurstType { get; set; } = 0;
    public uint Foe2PilotId { get; set; } = 0;
    public uint Foe2MsId { get; set; } = 0;
    public uint Foe2EchelonId { get; set; } = 0;
    public uint Foe2BurstType { get; set; } = 0;
}