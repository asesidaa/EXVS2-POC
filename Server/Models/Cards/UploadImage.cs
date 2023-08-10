using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.Cards;

[Table("upload_image")]
public class UploadImage : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ImageId { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required]
    public string Filename { get; set; } = string.Empty;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}