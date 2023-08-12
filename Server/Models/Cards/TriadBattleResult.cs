using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.Cards;

[Table("triad_battle_result")]
public class TriadBattleResult : BaseBattleResult
{
    [Required]
    public uint CourseId { get; set; } = 0;
    
    [Required]
    public uint SceneId { get; set; } = 0;
}