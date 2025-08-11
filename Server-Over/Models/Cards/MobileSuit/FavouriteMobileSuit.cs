using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServerOver.Models.Cards.Titles.MobileSuit;

namespace ServerOver.Models.Cards.MobileSuit;

[Table("exvs2ob_favourite_ms")]
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

    [Required]
    public uint MstMobileSuitId { get; set; } = 0;
    
    [Required]
    public bool OpenSkillpoint { get; set; } = true;
    
    [Required]
    public uint GaugeDesignId { get; set; } = 0;
    
    public string BgmSettings { get; set; } = string.Empty;
    
    [Required]
    public uint BgmPlayMethod { get; set; } = 0;
    
    [Required]
    public uint BattleNavId { get; set; } = 0;
    
    [Required]
    public uint BurstType { get; set; } = 0;

    [Required] 
    public MobileSuitDefaultTitle MobileSuitDefaultTitle { get; set; } = new();
    
    [Required] 
    public MobileSuitClassMatchTitle MobileSuitClassMatchTitle { get; set; } = new();
    
    [Required] 
    public MobileSuitTriadTitle MobileSuitTriadTitle { get; set; } = new();
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}