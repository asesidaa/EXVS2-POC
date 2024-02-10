using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Common;

[Table("exvs2_common_echelon_setting")]
[Index(nameof(Id))]
public class EchelonSetting : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    [Required] 
    public uint EchelonId { get; set; } = 0;
    [Required] 
    public uint OnlineMatchRankS { get; set; } = 0;
    [Required] 
    public uint OnlineMatchRankT { get; set; } = 0;
    [Required] 
    public uint OnlineMatchRankE { get; set; } = 0;
    [Required] 
    public bool MsAdjustFlag { get; set; } = true;
    [Required] 
    public int DownDefaultExp { get; set; } = 0;
    [Required] 
    public int UpDefaultExp { get; set; } = 0;
    [Required] 
    public uint WinCorrectionRate { get; set; } = 0;
    [Required] 
    public uint LoseCorrectionRate { get; set; } = 0;
    [Required] 
    public int ExpWidth { get; set; } = 0;
    [Required] 
    public int DowngradeThreshold { get; set; } = 0;
}