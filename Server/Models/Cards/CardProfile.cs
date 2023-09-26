namespace Server.Models.Cards;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("card_profile")]
public class CardProfile : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public string AccessCode { get; set; } = string.Empty;

    [Required] 
    public string ChipId { get; set; } = string.Empty;
    
    [Required]
    public string SessionId { get; set; } = string.Empty;

    [Required] 
    public Boolean IsNewCard { get; set; } = true;
    
    [Required]
    public string UploadToken { get; set; } = string.Empty;

    [Required]
    public DateTime UploadTokenExpiry { get; set; } = DateTime.Now;
    
    public ICollection<UploadImage> UploadImages { get; } = new List<UploadImage>(); 
    
    public ICollection<OnlinePair> OnlinePairs { get; } = new List<OnlinePair>(); 
    
    public int QuickOnlinePartnerId { get; set; } = 0;
    
    public ICollection<TriadBattleResult> TriadBattleResults { get; } = new List<TriadBattleResult>(); 
    
    public ICollection<OfflinePvpBattleResult> OfflinePvpBattleResults { get; } = new List<OfflinePvpBattleResult>(); 

    public virtual PilotDomain PilotDomain { get; set; } = null!;

    public virtual UserDomain UserDomain { get; set; } = null!;
}