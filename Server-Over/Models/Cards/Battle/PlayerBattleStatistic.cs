using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Battle;

[Table("exvs2ob_player_battle_statistic")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class PlayerBattleStatistic : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required] 
    public uint TotalGivenDamage { get; set; } = 0;
    
    [Required] 
    public uint TotalEnemyDefeatedCount { get; set; } = 0;
    
    [Required] 
    public uint TotalClassMatchTenConsecutiveWinCount { get; set; } = 0;
    
    [Required] 
    public uint TotalNoDamageBattleCount { get; set; } = 0;
    
    [Required] 
    public uint TotalExBurstDamage { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}