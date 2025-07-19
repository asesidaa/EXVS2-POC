using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Titles.User;

[Table("exvs2ob_user_title_setting")]
[Index(nameof(UserTitleSettingId))]
[Index(nameof(CardId))]
public class UserTitleSetting
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserTitleSettingId { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required] 
    public UserDefaultTitle UserDefaultTitle { get; set; } = new();
    
    [Required] 
    public UserTriadTitle UserTriadTitle { get; set; } = new();
    
    [Required] 
    public UserClassMatchTitle UserClassMatchTitle { get; set; } = new();
    
    [Required] 
    public bool RandomTitleFlag { get; set; } = false;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}