using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Settings;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_gamepad_setting")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class GamepadSetting : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    public uint XKey { get; set; } = 1;
    public uint YKey { get; set; } = 2;
    public uint AKey { get; set; } = 3;
    public uint BKey { get; set; } = 4;
    public uint LbKey { get; set; } = 5;
    public uint RbKey { get; set; } = 6;
    public uint LtKey { get; set; } = 7;
    public uint RtKey { get; set; } = 8;
    public uint LsbKey { get; set; } = 0;
    public uint RsbKey { get; set; } = 9;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}