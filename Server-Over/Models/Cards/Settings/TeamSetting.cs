using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServerOver.Models.Cards.Team;

namespace ServerOver.Models.Cards.Settings;

[Table("exvs2ob_team_setting")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class TeamSetting : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required] 
    public uint QuickOnlineTagCardId { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}