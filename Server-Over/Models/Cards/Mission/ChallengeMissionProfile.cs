using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Mission;

[Table("exvs2ob_challenge_mission_profile")]
[Index(nameof(Id))]
[Index(nameof(CardId), nameof(EffectiveYear), nameof(EffectiveMonth), nameof(EffectiveDay))]
public class ChallengeMissionProfile : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required] 
    public uint EffectiveYear { get; set; } = 0;

    [Required] 
    public uint EffectiveMonth { get; set; } = 0;

    [Required] 
    public uint EffectiveDay { get; set; } = 0;

    public uint TotalBattleCount { get; set; } = 0;
    public uint TotalBattleWinCount { get; set; } = 0;
    public uint MaxConsecutiveWinCount { get; set; } = 0;
    public uint TotalDefeatCount { get; set; } = 0;
    public uint TotalDamageCount { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}