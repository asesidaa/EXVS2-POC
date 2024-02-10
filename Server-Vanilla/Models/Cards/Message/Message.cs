using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerVanilla.Models.Cards.Message;

public class Message : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    public string TopMessageText { get; set; } = string.Empty;
    public uint TopUniqueMessageId { get; set; } = 0;
    public string DownMessageText { get; set; } = string.Empty;
    public uint DownUniqueMessageId { get; set; } = 0;
    public string LeftMessageText { get; set; } = string.Empty;
    public uint LeftUniqueMessageId { get; set; } = 0;
    public string RightMessageText { get; set; } = string.Empty;
    public uint RightUniqueMessageId { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}