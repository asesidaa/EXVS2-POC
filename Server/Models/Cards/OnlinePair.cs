namespace Server.Models.Cards;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("online_pair")]
public class OnlinePair
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PairId { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required]
    public int TeammateCardId { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}