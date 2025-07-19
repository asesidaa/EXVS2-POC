using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Triad;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2ob_triad_partner")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class TriadPartner : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required]
    public uint MstMobileSuitId { get; set; } = 0;
    
    [Required]
    public uint ArmorLevel { get; set; } = 0;
    
    [Required]
    public uint ShootAttackLevel { get; set; } = 0;
    
    [Required]
    public uint InfightAttackLevel { get; set; } = 0;
    
    [Required]
    public uint BoosterLevel { get; set; } = 0;
    
    [Required]
    public uint ExGaugeLevel { get; set; } = 0;
    
    [Required]
    public uint AiLevel { get; set; } = 0;
    
    [Required]
    public uint BurstType { get; set; } = 0;
    
    [Required]
    public uint MsSkill1 { get; set; } = 0;
    
    [Required]
    public uint MsSkill2 { get; set; } = 0;

    [Required]
    public string TriadTeamName { get; set; } = "EXTREME TEAM";
    
    [Required]
    public uint TriadBackgroundPartsId { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}