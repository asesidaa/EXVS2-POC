using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.MobileSuit;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_favourite_ms")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
[Index(nameof(CardId), nameof(MstMobileSuitId))]
public class FavouriteMobileSuit : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    public uint MstMobileSuitId { get; set; } = 0;
    public bool OpenSkillpoint { get; set; } = true;
    public uint GaugeDesignId { get; set; } = 0;
    public string BgmSettings { get; set; } = string.Empty;
    public uint BgmPlayMethod { get; set; } = 0;
    public uint BattleNavId { get; set; } = 0;
    public uint BurstType { get; set; } = 2;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}