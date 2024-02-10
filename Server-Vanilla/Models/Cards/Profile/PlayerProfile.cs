using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Profile;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_player_profile")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class PlayerProfile : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    public uint OpenRecord { get; set; } = 1;
    public uint OpenEchelon { get; set; } = 0;
    public bool OpenSkillpoint { get; set; } = true;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}