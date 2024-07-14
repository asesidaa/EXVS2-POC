using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Server.Models.Cards;

namespace Server.Models.Common;

[Table("common_echelon_setting")]
[Index(nameof(Id), nameof(EchelonId))]
public class EchelonSetting : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    [Required] 
    public uint EchelonId { get; set; } = 0;
    [Required]
    [StringLength(20)]
    public string EchelonName { get; set; } = string.Empty;
    [Required] 
    public uint ExpWidth { get; set; } = 0;
    [Required] 
    public int UpDefaultExp { get; set; } = 0;
    [Required] 
    public int DownDefaultExp { get; set; } = 0;
    [Required] 
    public int DowngradeThreshold { get; set; } = 0;
}