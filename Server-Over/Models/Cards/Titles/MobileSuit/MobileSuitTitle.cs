using System.ComponentModel.DataAnnotations;
using ServerOver.Models.Cards.MobileSuit;

namespace ServerOver.Models.Cards.Titles.MobileSuit;

public class MobileSuitTitle : BaseTitle
{
    [Required]
    public int FavouriteMobileSuitId { get; set; }
    
    public virtual FavouriteMobileSuit FavouriteMobileSuit { get; set; } = null!;
}