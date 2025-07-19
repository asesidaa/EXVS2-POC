using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Settings;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2ob_navi_setting")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class NaviSetting : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required]
    public bool BattleNavAdviseFlag { get; set; } = true;
    
    [Required]
    public bool BattleNavNotifyFlag { get; set; } = true;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}