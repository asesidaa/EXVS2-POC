using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Triad;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_triad_course_data")]
[Index(nameof(Id))]
[Index(nameof(CardId), nameof(CourseId))]
public class TriadCourseData : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    public uint CourseId { get; set; } = 0;
    public ulong ReleasedAt { get; set; } = 0;
    public uint TotalPlayNum { get; set; } = 0;
    public uint TotalClearNum { get; set; } = 0;
    public uint Highscore { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}