using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Profile;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2ob_customize_profile")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class CustomizeProfile : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required]
    public uint DefaultGaugeDesignId { get; set; } = 0;
    
    public string DefaultBgmSettings { get; set; } = string.Empty;
    
    [Required]
    public uint DefaultBgmPlayMethod { get; set; } = 0;
    
    [Required]
    public string StageRandoms { get; set; } = string.Empty;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}