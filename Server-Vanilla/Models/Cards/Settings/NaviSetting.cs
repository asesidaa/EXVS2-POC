using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Settings;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_navi_setting")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class NaviSetting : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    public bool BattleNavAdviseFlag { get; set; } = true;
    public bool BattleNavNotifyFlag { get; set; } = true;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}