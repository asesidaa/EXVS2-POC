using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Triad;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_triad_partner")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class TriadPartner : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    public uint MstMobileSuitId { get; set; } = 0;
    public uint ArmorLevel { get; set; } = 0;
    public uint ShootAttackLevel { get; set; } = 0;
    public uint InfightAttackLevel { get; set; } = 0;
    public uint BoosterLevel { get; set; } = 0;
    public uint ExGaugeLevel { get; set; } = 0;
    public uint AiLevel { get; set; } = 0;
    public uint BurstType { get; set; } = 0;
    public uint MsSkill1 { get; set; } = 0;
    public uint MsSkill2 { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}