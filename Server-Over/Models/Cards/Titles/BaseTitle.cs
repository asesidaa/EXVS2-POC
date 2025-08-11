using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerOver.Models.Cards.Titles;

public class BaseTitle : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public uint TitleTextId { get; set; } = 0;
    
    [Required]
    public uint TitleOrnamentId { get; set; } = 0;
    
    [Required]
    public uint TitleEffectId { get; set; } = 0;
    
    [Required]
    public uint TitleBackgroundPartsId { get; set; } = 0;
    
    public string CustomText { get; set; } = string.Empty;
}