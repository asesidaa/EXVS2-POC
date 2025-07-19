using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ServerOver.Models.Cards.Settings;

namespace ServerOver.Models.Cards.Message;

public class Message : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int MessageSettingId { get; set; }

    public string TopMessageText { get; set; } = string.Empty;
    
    [Required]
    public uint TopUniqueMessageId { get; set; } = 0;
    
    public string DownMessageText { get; set; } = string.Empty;
    
    [Required]
    public uint DownUniqueMessageId { get; set; } = 0;
    
    public string LeftMessageText { get; set; } = string.Empty;
    
    [Required]
    public uint LeftUniqueMessageId { get; set; } = 0;
    
    public string RightMessageText { get; set; } = string.Empty;
    
    [Required]
    public uint RightUniqueMessageId { get; set; } = 0;
    
    public virtual MessageSetting MessageSetting { get; set; } = null!;
}