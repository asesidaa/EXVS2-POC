using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Settings;

[Table("exvs2ob_boost_setting")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class BoostSetting : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required]
    public uint GpBoost { get; set; } = 1;
    
    [Required]
    public uint GuestNavBoost { get; set; } = 1;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}