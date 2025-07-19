using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Navi;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2ob_navi")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
[Index(nameof(CardId), nameof(GuestNavId))]
public class Navi : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required]
    public uint GuestNavId { get; set; } = 0;
    
    [Required]
    public bool GuestNavSettingFlag { get; set; } = false;
    
    [Required]
    public uint GuestNavRemains { get; set; } = 9999;
    
    [Required]
    public bool BattleNavSettingFlag { get; set; } = false;
    
    [Required]
    public uint BattleNavRemains { get; set; } = 9999;
    
    [Required]
    public uint GuestNavCostume { get; set; } = 0;
    
    [Required]
    public uint GuestNavFamiliarity { get; set; } = 0;
    
    [Required]
    public bool NewCostumeFlag { get; set; } = false;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}