namespace Server.Models.Cards;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("card_profile")]
public class CardProfile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public string AccessCode { get; set; } = string.Empty;

    [Required] 
    public string ChipId { get; set; } = string.Empty;
    
    [Required]
    public string SessionId { get; set; } = string.Empty;

    [Required] 
    public Boolean IsNewCard { get; set; } = true;

    public virtual PilotDomain PilotDomain { get; set; } = null!;

    public virtual UserDomain UserDomain { get; set; } = null!;
}