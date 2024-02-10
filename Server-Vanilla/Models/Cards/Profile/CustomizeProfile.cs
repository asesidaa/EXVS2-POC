using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Profile;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_customize_profile")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class CustomizeProfile : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    public uint DefaultGaugeDesignId { get; set; } = 0;
    public string DefaultBgmSettings { get; set; } = string.Empty;
    public uint DefaultBgmPlayMethod { get; set; } = 0;
    public string StageRandoms { get; set; } = string.Empty;
    public uint BasePanelId { get; set; } = 0;
    public uint CommentPartsAId { get; set; } = 0;
    public uint CommentPartsBId { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}