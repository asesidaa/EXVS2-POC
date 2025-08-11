using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerOver.Models.Cards.Team;

[Table("exvs2ob_tag_team_data")]
public class TagTeamData : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required] 
    public string TeamName { get; set; } = string.Empty;
    
    [Required]
    public uint TeammateCardId { get; set; } = 0;
    
    [Required]
    public int OnlineRankPoint { get; set; } = 0;
    
    [Required]
    public uint BackgroundPartsId { get; set; } = 0;
    
    [Required]
    public uint EffectId { get; set; } = 0;
    
    [Required]
    public uint EmblemId { get; set; } = 0;
    
    [Required]
    public uint SkillPoint { get; set; } = 0;
    
    [Required]
    public uint SkillPointBoost { get; set; } = 0;
    
    [Required]
    public uint BgmId { get; set; } = 0;
    
    [Required]
    public uint NameColorId { get; set; } = 0;

    [Required] 
    public uint BoostRemains { get; set; } = 9999;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}