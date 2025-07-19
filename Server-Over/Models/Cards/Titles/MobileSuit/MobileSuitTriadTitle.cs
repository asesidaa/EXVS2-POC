using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Titles.MobileSuit;

[Table("exvs2ob_ms_triad_title")]
[Index(nameof(Id))]
[Index(nameof(FavouriteMobileSuitId))]
public class MobileSuitTriadTitle : MobileSuitTitle
{
}