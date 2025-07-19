using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Replay;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("exvs2ob_upload_replay")]
[Index(nameof(ReplayId))]
[Index(nameof(CardId), nameof(ReplayId))]
public class UploadReplay : BaseEntity
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
    
    [Required] 
    public bool ReplayServiceFlag { get; set; } = false;

    [Required] 
    public uint MobileUserId { get; set; } = 0;

    [Required] 
    public uint MatchingMode { get; set; } = 0;

    [Required] 
    public uint TeamType { get; set; } = 0;

    [Required] 
    public bool ReturnMatchFlag { get; set; } = false;
    
    public uint? TournamentId { get; set; }
    
    [Required] 
    public uint BattleClass { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}