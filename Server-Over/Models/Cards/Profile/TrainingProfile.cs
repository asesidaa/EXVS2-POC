using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Profile;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2ob_training_profile")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class TrainingProfile : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required]
    public uint MstMobileSuitId { get; set; } = 1;
    
    [Required]
    public uint BurstType { get; set; } = 0;
    
    [Required]
    public uint CpuLevel { get; set; } = 1;
    
    [Required]
    public uint ExBurstGauge { get; set; } = 0;
    
    [Required]
    public bool DamageDisplay { get; set; } = true;
    
    [Required]
    public bool CpuAutoGuard { get; set; } = true;
    
    [Required]
    public bool CommandGuideDisplay { get; set; } = true;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}