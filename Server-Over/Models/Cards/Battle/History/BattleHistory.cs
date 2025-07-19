using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Battle.History;

[Table("exvs2ob_battle_history")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
[Index(nameof(PlayedAt))]
[Index(nameof(Id), nameof(CardId))]
public class BattleHistory : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required]
    public string BattleMode { get; set; } = string.Empty;
    
    [Required]
    public bool IsWin { get; set; }
    
    [Required]
    public ulong PlayedAt { get; set; } = 0;
    
    [Required]
    public uint ElapsedSeconds { get; set; } = 0;

    [Required] 
    public uint TeamId { get; set; } = 0;
    
    [Required]
    public uint StageId { get; set; } = 0;

    [Required] 
    public uint Score { get; set; } = 0;

    [Required] 
    public uint ScoreRank { get; set; } = 0;

    [Required] 
    public uint BurstType { get; set; } = 0;
    
    [Required] 
    public uint BurstCount { get; set; } = 0;
    
    [Required] 
    public uint TotalExBurstDamage { get; set; } = 0;
    
    [Required] 
    public uint GivenDamage { get; set; } = 0;
    
    [Required] 
    public uint TakenDamage { get; set; } = 0;
    
    [Required] 
    public uint OverheatCount { get; set; } = 0;

    [Required] 
    public uint ComboGivenDamage { get; set; } = 0;

    [Required] 
    public uint ConsecutiveWinCount { get; set; } = 0;

    [Required] 
    public BattleSelf BattleSelf { get; set; } = new();

    public BattleAlly? Ally { get; set; } = null;
    
    public ICollection<BattleTarget> Targets { get; set; } = new List<BattleTarget>();
    
    public ICollection<BattleActionLog> ActionLogs { get; set; } = new List<BattleActionLog>();
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}