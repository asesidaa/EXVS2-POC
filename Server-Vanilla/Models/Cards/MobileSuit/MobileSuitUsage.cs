using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.MobileSuit;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_ms_usage")]
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
    
    public uint MstMobileSuitId { get; set; } = 0;
    public uint MsUsedNum { get; set; } = 0;
    public uint CostumeId { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}