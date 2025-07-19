using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Upload;

[Table("exvs2ob_upload_image")]
[Index(nameof(ImageId))]
[Index(nameof(CardId), nameof(ImageId))]
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