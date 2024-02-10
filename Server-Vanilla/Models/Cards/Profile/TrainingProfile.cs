using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Profile;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_training_profile")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class TrainingProfile : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    public uint MstMobileSuitId { get; set; } = 1;
    public uint BurstType { get; set; } = 2;
    public uint CpuLevel { get; set; } = 1;
    public uint ExBurstGauge { get; set; } = 0;
    public bool DamageDisplay { get; set; } = true;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}