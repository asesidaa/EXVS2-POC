using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.MobileSuit;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2ob_ms_usage")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
[Index(nameof(CardId), nameof(MstMobileSuitId))]
public class MobileSuitUsage : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required]
    public uint MstMobileSuitId { get; set; } = 0;
    
    [Required]
    public uint MsUsedNum { get; set; } = 0;
    
    [Required]
    public uint CostumeId { get; set; } = 0;

    [Required]
    public uint TriadBuddyPoint { get; set; } = 0;
    
    [Required]
    public uint SkinId { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}