using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Triad.Ranking;

[Table("exvs2ob_triad_high_score")]
[Index(nameof(Id))]
[Index(nameof(CardId), nameof(Year), nameof(Month))]
public class TriadHighScore : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required]
    public uint Year { get; set; }
    
    [Required]
    public uint Month { get; set; }

    [Required] 
    public uint CourseHighScore { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}