using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServerOver.Models.Cards.Room;

namespace ServerOver.Models.Cards.Settings;

[Table("exvs2ob_user_private_match_room_setting")]
[Index(nameof(PrivateMatchRoomSettingId))]
[Index(nameof(CardId))]
public class PrivateMatchRoomSetting
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PrivateMatchRoomSettingId { get; }
    
    [Microsoft.Build.Framework.Required]
    public int CardId { get; set; }
    
    [Microsoft.Build.Framework.Required]
    public bool EnablePrivateMatch { get; set; } = false;
    
    [Microsoft.Build.Framework.Required]
    public bool IsPrivateMatchHost { get; set; } = false;
    
    [Microsoft.Build.Framework.Required]
    public int ParticipatedPrivateRoomId { get; set; } = 0;

    [Microsoft.Build.Framework.Required] 
    public PrivateMatchRoom SelfPrivateRoomConfig { get; set; } = new();
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}