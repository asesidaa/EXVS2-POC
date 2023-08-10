namespace Server.Models.Cards;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("user_domain")]
public class UserDomain : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required]
    public string UserJson { get; set; } = string.Empty;
    
    [Required]
    public string MobileUserGroupJson { get; set; } = string.Empty;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}