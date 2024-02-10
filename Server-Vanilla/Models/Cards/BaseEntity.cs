using System.ComponentModel.DataAnnotations;

namespace ServerVanilla.Models.Cards;

public class BaseEntity
{
    [Required]
    public DateTime CreateTime { get; set; } = DateTime.Now;
    
    [Required]
    public DateTime UpdateTime { get; set; } = DateTime.Now;
}