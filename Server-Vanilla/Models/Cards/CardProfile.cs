using Microsoft.EntityFrameworkCore;
using ServerVanilla.Models.Cards.Battle;
using ServerVanilla.Models.Cards.Message;
using ServerVanilla.Models.Cards.MobileSuit;
using ServerVanilla.Models.Cards.Profile;
using ServerVanilla.Models.Cards.Replay;
using ServerVanilla.Models.Cards.Settings;
using ServerVanilla.Models.Cards.Team;
using ServerVanilla.Models.Cards.Title;
using ServerVanilla.Models.Cards.Triad;

namespace ServerVanilla.Models.Cards;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2_card_profile")]
[Index(nameof(Id))]
public class CardProfile : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }

    [Required] 
    public string UserName { get; set; } = string.Empty;
    
    [Required] 
    public uint Gp { get; set; } = 10000;
    
    [Required]
    public string AccessCode { get; set; } = string.Empty;

    [Required] 
    public string ChipId { get; set; } = string.Empty;
    
    [Required]
    public string SessionId { get; set; } = string.Empty;

    [Required] 
    public bool IsNewCard { get; set; } = true;
    
    [Required]
    public string UploadToken { get; set; } = string.Empty;

    [Required]
    public DateTime UploadTokenExpiry { get; set; } = DateTime.Now;
    
    [Required]
    public string DistinctTeamFormationToken { get; set; } = Guid.NewGuid().ToString("n").Substring(0, 16);

    [Required]
    public ulong LastPlayedAt { get; set; } = 0;

    [Required] 
    public PlayerProfile PlayerProfile { get; set; } = new();
    
    [Required] 
    public BattleProfile BattleProfile { get; set; } = new();
    
    [Required] 
    public CustomizeProfile CustomizeProfile { get; set; } = new();
    
    [Required] 
    public TrainingProfile TrainingProfile { get; set; } = new();
    
    // MS and Navi Domain
    public ICollection<MobileSuitUsage> MobileSuits { get; } = new List<MobileSuitUsage>(); 
    public ICollection<FavouriteMobileSuit> FavouriteMobileSuits { get; } = new List<FavouriteMobileSuit>();
    public ICollection<Navi.Navi> Navi { get; } = new List<Navi.Navi>(); 
    
    // Triad Domain
    [Required] 
    public TriadMiscInfo TriadMiscInfo { get; set; } = new();
    
    [Required] 
    public TriadPartner TriadPartner { get; set; } = new();
    
    public ICollection<TriadCourseData> TriadCourseDatas { get; } = new List<TriadCourseData>();
    
    // Setting Domain
    [Required] 
    public BoostSetting BoostSetting { get; set; } = new();
    [Required] 
    public NaviSetting NaviSetting { get; set; } = new();
    [Required] 
    public GamepadSetting GamepadSetting { get; set; } = new();
    
    // Team Domain
    public ICollection<TagTeamData> TagTeamDatas { get; } = new List<TagTeamData>(); 
    public ICollection<OnlinePair> OnlinePairs { get; } = new List<OnlinePair>(); 
    
    // Title Domain
    [Required] 
    public DefaultTitle DefaultTitle { get; set; } = new();
    
    // Message Domain
    [Required] 
    public OpeningMessage OpeningMessage { get; set; } = new();
    [Required] 
    public PlayingMessage PlayingMessage { get; set; } = new();
    [Required] 
    public ResultMessage ResultMessage { get; set; } = new();
    [Required] 
    public OnlineOpeningMessage OnlineOpeningMessage { get; set; } = new();
    [Required] 
    public OnlinePlayingMessage OnlinePlayingMessage { get; set; } = new();
    [Required] 
    public OnlineResultMessage OnlineResultMessage { get; set; } = new();
    
    // Replay Domain
    public ICollection<UploadReplay> UploadReplays { get; } = new List<UploadReplay>(); 
    public ICollection<SharedUploadReplay> SharedUploadReplays { get; } = new List<SharedUploadReplay>(); 

    // Battle Result Domain
    public ICollection<OfflinePvpBattleResult> OfflinePvpBattleResults { get; } = new List<OfflinePvpBattleResult>(); 
}