using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Titles.User;

[Table("exvs2ob_user_default_title")]
[Index(nameof(Id))]
[Index(nameof(UserTitleSettingId))]
public class UserDefaultTitle : UserTitle
{
    
}