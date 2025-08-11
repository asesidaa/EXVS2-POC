using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Settings;

[Table("exvs2ob_general_setting")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class GeneralSetting : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required] 
    public bool FixPositionRadar { get; set; } = false; 
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}