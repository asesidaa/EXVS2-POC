using System.ComponentModel.DataAnnotations;

namespace ServerOver.Models.Cards.Titles.User;

public class UserTitle : BaseTitle
{
    [Required]
    public int UserTitleSettingId { get; set; }
    
    public virtual UserTitleSetting UserTitleSetting { get; set; } = null!;
}