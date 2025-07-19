using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Triad;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2ob_triad_misc_info")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class TriadMiscInfo : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required]
    public string CpuRibbons { get; set; } = string.Empty;
    
    [Required]
    public uint TotalTriadScore { get; set; }
    
    [Required]
    public uint TotalTriadWantedDefeatNum { get; set; }
    
    [Required]
    public uint TotalTriadScenePlayNum { get; set; }
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}