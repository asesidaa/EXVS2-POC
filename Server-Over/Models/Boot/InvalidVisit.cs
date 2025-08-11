using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Boot;

[Table("exvs2ob_invalid_visit")]
[Index(nameof(Id))]
public class InvalidVisit : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }

    [StringLength(384)]
    [Required]
    public string Token { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string Ip { get; set; } = string.Empty;
}