using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Battle;

[Table("exvs2ob_battle_player_level")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class PlayerLevel : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required]
    public uint PlayerLevelId { get; set; } = 1;

    [Required]
    // 0 = Normal Player Level, 1 = Gold, 2 = Rainbow
    public uint PrestigeId { get; set; } = 0;

    [Required]
    public uint PlayerExp { get; set; } = 0;

    [Required] 
    // True = Will be special alert
    public bool LevelMaxDispFlag { get; set; } = false;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}