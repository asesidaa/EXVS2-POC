using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.MobileSuit;

[Table("exvs2ob_ms_sticker")]
[Index(nameof(StickerId))]
[Index(nameof(CardId))]
[Index(nameof(CardId), nameof(MstMobileSuitId))]
public class MobileSuitSticker : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StickerId { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required]
    public uint MstMobileSuitId { get; set; } = 0;

    [Required]
    public uint PoseId { get; set; } = 0;
    
    [Required]
    public uint StickerBackgroundId { get; set; } = 0;

    [Required] 
    public uint StickerEffectId { get; set; } = 0;
    
    [Required] 
    public uint Tracker1 { get; set; } = 0;
    
    [Required] 
    public uint Tracker2 { get; set; } = 0;
    
    [Required] 
    public uint Tracker3 { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}