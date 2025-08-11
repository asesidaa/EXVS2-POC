using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerOver.Models.Cards.Battle;

public class BaseClassMatchRecord : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required] 
    public uint ClassId { get; set; } = 1;

    [Required] 
    public float Rate { get; set; } = 1500.0f;
    
    [Required] 
    public uint MaxPosition { get; set; } = 0;
    
    [Required] 
    // 0 = Nothing, 1 = Upgraded, 2 = Downgraded
    // Will be displayed in PreLoadCard
    public uint ClassChangeStatus { get; set; } = 0;

    // Special Upgrade Methods:
    // Having 20 Battles or above, and having win rate at 100%
    // or Having 30 Battles or above, and having win rate at 70%
    // Then can upgrade automatically
    [Required] 
    public uint WeeklyTotalBattleCount { get; set; } = 0;

    [Required] 
    public uint WeeklyTotalWinCount { get; set; } = 0;
    
    // V10 or after:
    // Ripple effect on Rank Blade if = 1
    // Thunderbolt effect on Rank Blade if = 2
    // Strongest Thunderbolt effect on Rank Blade if = 3
    // All Effective in Over Rank only
    [Required] 
    public uint TopPointRankEntryCount { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}