using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Battle;

[Table("exvs2ob_battle_win_loss_record")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class WinLossRecord : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required]
    public uint TotalWin { get; set; } = 0;

    [Required]
    public uint TotalLose { get; set; } = 0;
    
    [Required]
    public uint ShuffleWin { get; set; } = 0;

    [Required] 
    public uint ShuffleLose { get; set; } = 0;
    
    [Required]
    public uint TeamWin { get; set; } = 0;

    [Required]
    public uint TeamLose { get; set; } = 0;

    [Required]
    public uint ClassSoloWin { get; set; } = 0;

    [Required]
    public uint ClassSoloLose { get; set; } = 0;

    [Required]
    public uint ClassTeamWin { get; set; } = 0;

    [Required]
    public uint ClassTeamLose { get; set; } = 0;
    
    [Required]
    public uint FesWin { get; set; } = 0;
    
    [Required]
    public uint FesLose { get; set; } = 0;
    
    [Required]
    public uint FreeWin { get; set; } = 0;
    
    [Required]
    public uint FreeLose { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}