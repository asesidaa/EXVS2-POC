using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServerOver.Models.Cards.Settings;

namespace ServerOver.Models.Cards.Room;

[Table("exvs2ob_private_match_room")]
[Index(nameof(Id))]
[Index(nameof(PrivateMatchRoomSettingId))]
public class PrivateMatchRoom : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int PrivateMatchRoomSettingId { get; set; }

    public string TagName { get; set; } = Guid.NewGuid().ToString("n").Substring(0, 10);

    [Required]
    public uint TagType { get; set; } = 1;
    
    [Required]
    public uint MatchingType { get; set; } = 1;
    
    [Required]
    public uint MatchingAttribute { get; set; } = 1;
    
    [Required]
    public uint RuleType { get; set; } = 1;

    [Required]
    public string SelectableMsIds { get; set; } = string.Empty;

    [Required]
    public bool RevengeFlag { get; set; } = false;
    
    public virtual PrivateMatchRoomSetting PrivateMatchRoomSetting { get; set; } = null!;
}