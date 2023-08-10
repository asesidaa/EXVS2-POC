namespace Server.Models.Cards;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("pilot_domain")]
public class PilotDomain : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PilotId { get; }

    [Required]
    public int CardId { get; set; }

    [Required]
    public string LoadPlayerJson { get; set; } = string.Empty;
    
    [Required]
    public string PilotDataGroupJson { get; set; } = string.Empty;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}