using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Profile;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_battle_profile")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class BattleProfile : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    public uint TotalWin { get; set; } = 0;
    public uint TotalLose { get; set; } = 0;
    public uint EchelonId { get; set; } = 0;
    public int EchelonExp { get; set; } = 0;
    public bool SEchelonFlag { get; set; } = false;
    public bool SEchelonMissionFlag { get; set; } = false;
    public uint SEchelonProgress { get; set; } = 0;
    public bool SCaptainFlag { get; set; } = false;
    public bool SBrigadierFlag { get; set; } = false;
    public int MatchingCorrectionSolo { get; set; } = 0;
    public int MatchingCorrectionTeam { get; set; } = 0;
    public uint VsmAfterRankUp { get; set; } = 0;
    public uint ShuffleWin { get; set; } = 0;
    public uint ShuffleLose { get; set; } = 0;
    public uint TeamWin { get; set; } = 0;
    public uint TeamLose { get; set; } = 0;
    
    // For the following 2 fields, they originally doesn't exist
    public int SoloRankPoint { get; set; } = 0;
    public int TeamRankPoint { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}