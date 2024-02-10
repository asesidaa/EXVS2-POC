using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Triad;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_triad_misc_info")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class TriadMiscInfo : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    public string CpuRibbons { get; set; } = string.Empty;
    public uint TotalTriadScore { get; set; }
    public uint TotalTriadWantedDefeatNum { get; set; }
    public uint TotalTriadScenePlayNum { get; set; }
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}