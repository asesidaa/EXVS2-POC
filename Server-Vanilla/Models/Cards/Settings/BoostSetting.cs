using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Settings;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_boost_setting")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class BoostSetting : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    public uint GpBoost { get; set; } = 1;
    public uint GuestNavBoost { get; set; } = 1;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}