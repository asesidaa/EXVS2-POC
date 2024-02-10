using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Replay;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("exvs2_shared_upload_replay")]
[Index(nameof(ReplayId))]
[Index(nameof(CardId))]
public class SharedUploadReplay : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReplayId { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required]
    public string Filename { get; set; } = string.Empty;

    [Required]
    public uint ReplaySize { get; set; } = 0;

    [Required]
    public ulong PlayedAt { get; set; } = 0;

    [Required] 
    public uint StageId { get; set; } = 0;
    
    [Required]
    public string PilotsJson { get; set; } = string.Empty;

    [Required] 
    public bool SpecialFlag { get; set; } = false;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}