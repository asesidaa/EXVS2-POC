using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.MobileSuit;

[Table("exvs2ob_ms_pvp_stat")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
[Index(nameof(CardId), nameof(MstMobileSuitId))]
public class MobileSuitPvPStatistic : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required]
    public uint MstMobileSuitId { get; set; } = 0;

    [Required] 
    public uint TotalBattleCount { get; set; } = 0;
    
    [Required] 
    public uint TotalWinCount { get; set; } = 0;
    
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