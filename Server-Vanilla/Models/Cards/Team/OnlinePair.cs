namespace ServerVanilla.Models.Cards.Team;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("exvs2_online_pair")]
public class OnlinePair : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PairId { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required]
    public int TeamId { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}