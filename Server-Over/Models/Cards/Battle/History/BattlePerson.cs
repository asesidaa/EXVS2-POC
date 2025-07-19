using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerOver.Models.Cards.Battle.History;

public class BattlePerson : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int BattleHistoryId { get; set; }
    
    [Required]
    public uint CardId { get; set; }
    
    [Required]
    public string PlayerName { get; set; } = string.Empty;
    
    [Required]
    public uint ClassId { get; set; }
    
    [Required]
    public uint PrestigeId { get; set; }
    
    [Required]
    public uint LevelId { get; set; }
    
    [Required]
    public uint MobileSuitId { get; set; }
    
    [Required]
    public uint SkinId { get; set; }
    
    [Required]
    public uint Mastery { get; set; }
    
    [Required]
    public uint BurstType { get; set; }
    
    public virtual BattleHistory BattleHistory { get; set; } = null!;
}