using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Battle;

[Table("exvs2ob_player_burst_statistic")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
[Index(nameof(CardId), nameof(BurstType))]
public class PlayerBurstStatistics : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required]
    public uint BurstType { get; set; }
    
    [Required]
    public uint TotalBattle { get; set; } = 0;
    
    [Required]
    public uint TotalWin { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}