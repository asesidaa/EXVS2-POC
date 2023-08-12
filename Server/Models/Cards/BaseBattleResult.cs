using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.Cards;

[NotMapped]
public class BaseBattleResult : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required] 
    public string Mode { get; set; } = "Triad";
    
    [Required] 
    public bool WinFlag { get; set; } = false;
    
    [Required] 
    public uint Score { get; set; } = 0;

    [Required] 
    public uint UsedMsId { get; set; } = 0;
    
    [Required] 
    public uint UsedBurstType { get; set; } = 0;

    [Required] 
    public uint ElapsedSecond { get; set; } = 0;

    [Required] 
    public uint PastEchelonId { get; set; } = 0;
    
    [Required] 
    public int EchelonExpChange { get; set; } = 0;
    
    [Required] 
    public uint EchelonIdAfterBattle { get; set; } = 0;
    
    [Required] 
    public int TotalEchelonExp { get; set; } = 0;

    [Required] 
    public string FullBattleResultJson { get; set; } = string.Empty;

    public virtual CardProfile CardProfile { get; set; } = null!;
}