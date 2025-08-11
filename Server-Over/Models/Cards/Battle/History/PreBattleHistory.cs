using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Battle.History;

[Table("exvs2ob_pre_battle_history")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class PreBattleHistory : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required]
    public uint CurrentConsecutiveWins { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}