using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Title;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_default_title")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class DefaultTitle : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    public uint TitleTextId { get; set; } = 0;
    public uint TitleOrnamentId { get; set; } = 0;
    public uint TitleEffectId { get; set; } = 0;
    public uint TitleBackgroundPartsId { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}