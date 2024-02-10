using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Navi;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_navi")]
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

    public uint GuestNavId { get; set; } = 0;
    public bool GuestNavSettingFlag { get; set; } = false;
    public uint GuestNavRemains { get; set; } = 9999;
    public bool BattleNavSettingFlag { get; set; } = false;
    public uint BattleNavRemains { get; set; } = 9999;
    public uint GuestNavCostume { get; set; } = 0;
    public uint GuestNavFamiliarity { get; set; } = 0;
    public bool NewCostumeFlag { get; set; } = false;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}