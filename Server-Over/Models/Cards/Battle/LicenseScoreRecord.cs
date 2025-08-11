using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Battle;

[Table("exvs2ob_license_score_record")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class LicenseScoreRecord : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required] 
    public uint LicenseScore { get; set; } = 0;
    
    [Required] 
    public int LastObtainedScore { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}