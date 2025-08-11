using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServerOver.Models.Cards.Message;

namespace ServerOver.Models.Cards.Settings;

[Table("exvs2ob_message_setting")]
[Index(nameof(MessageSettingId))]
[Index(nameof(CardId))]
public class MessageSetting : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MessageSettingId { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required] 
    public uint MessagePosition { get; set; } = 0;

    [Required] 
    public bool AllowReceiveMessage { get; set; } = true;
    
    [Required] 
    public OpeningMessage OpeningMessage { get; set; } = new();
    
    [Required] 
    public PlayingMessage PlayingMessage { get; set; } = new();
    
    [Required] 
    public ResultMessage ResultMessage { get; set; } = new();
    
    [Required] 
    public OnlineShuffleOpeningMessage OnlineShuffleOpeningMessage { get; set; } = new();
    
    [Required] 
    public OnlineShufflePlayingMessage OnlineShufflePlayingMessage { get; set; } = new();
    
    [Required] 
    public OnlineShuffleResultMessage OnlineShuffleResultMessage { get; set; } = new();
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}