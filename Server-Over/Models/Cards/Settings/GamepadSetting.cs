using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Settings;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2ob_gamepad_setting")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class GamepadSetting : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required]
    public uint XKey { get; set; } = 1;
    
    [Required]
    public uint YKey { get; set; } = 2;
    
    [Required]
    public uint AKey { get; set; } = 3;
    
    [Required]
    public uint BKey { get; set; } = 4;
    
    [Required]
    public uint LbKey { get; set; } = 5;
    
    [Required]
    public uint RbKey { get; set; } = 6;
    
    [Required]
    public uint LtKey { get; set; } = 7;
    
    [Required]
    public uint RtKey { get; set; } = 8;
    
    [Required]
    public uint LsbKey { get; set; } = 0;
    
    [Required]
    public uint RsbKey { get; set; } = 9;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}