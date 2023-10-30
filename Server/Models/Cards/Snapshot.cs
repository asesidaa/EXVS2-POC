using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.Cards;

[Table("snapshot")]
public class Snapshot : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }

    [Required] 
    public string SnapshotType { get; set; } = string.Empty;
    
    [Required] 
    public string SnapshotData { get; set; } = string.Empty;
}