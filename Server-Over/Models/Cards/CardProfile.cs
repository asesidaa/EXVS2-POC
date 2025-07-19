using Microsoft.EntityFrameworkCore;
using ServerOver.Models.Cards.Battle;
using ServerOver.Models.Cards.Battle.History;
using ServerOver.Models.Cards.Mission;
using ServerOver.Models.Cards.MobileSuit;
using ServerOver.Models.Cards.Profile;
using ServerOver.Models.Cards.Replay;
using ServerOver.Models.Cards.Settings;
using ServerOver.Models.Cards.Team;
using ServerOver.Models.Cards.Titles.User;
using ServerOver.Models.Cards.Triad;
using ServerOver.Models.Cards.Triad.Ranking;
using ServerOver.Models.Cards.Upload;

namespace ServerOver.Models.Cards;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("exvs2ob_card_profile")]
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

    // Profile Domain
    [Required] 
    public PlayerProfile PlayerProfile { get; set; } = new();
    
    [Required] 
    public CustomizeProfile CustomizeProfile { get; set; } = new();
    
    [Required] 
    public TrainingProfile TrainingProfile { get; set; } = new();
    
    [Required] 
    public DefaultStickerProfile DefaultStickerProfile { get; set; } = new();
    
    // Battle Domain
    [Required] 
    public PlayerLevel PlayerLevel { get; set; } = new();
    
    [Required] 
    public WinLossRecord WinLossRecord { get; set; } = new();
    
    [Required] 
    public SoloClassMatchRecord SoloClassMatchRecord { get; set; } = new();
    
    [Required] 
    public TeamClassMatchRecord TeamClassMatchRecord { get; set; } = new();
    
    [Required] 
    public PlayerBattleStatistic PlayerBattleStatistic { get; set; } = new();
    
    [Required] 
    public LicenseScoreRecord LicenseScoreRecord { get; set; } = new();

    [Required]
    public ICollection<PlayerBurstStatistics> PlayerBurstStatistics { get; set; } = new List<PlayerBurstStatistics>();
    
    // MS Domain
    public ICollection<MobileSuitUsage> MobileSuits { get; } = new List<MobileSuitUsage>(); 
    public ICollection<FavouriteMobileSuit> FavouriteMobileSuits { get; } = new List<FavouriteMobileSuit>();
    public ICollection<MobileSuitSticker> MobileSuitStickers { get; } = new List<MobileSuitSticker>();
    public ICollection<MobileSuitPvPStatistic> MobileSuitPvPStatistics { get; } = new List<MobileSuitPvPStatistic>();
    
    // Navi Domain
    public ICollection<Navi.Navi> Navis { get; } = new List<Navi.Navi>(); 
    
    // Triad Domain
    [Required] 
    public TriadMiscInfo TriadMiscInfo { get; set; } = new();
    
    [Required] 
    public TriadPartner TriadPartner { get; set; } = new();
    
    public ICollection<TriadCourseData> TriadCourseDatas { get; } = new List<TriadCourseData>();
    
    // Setting Domain
    [Required]
    public GeneralSetting GeneralSetting { get; set; } = new();
    
    [Required] 
    public BoostSetting BoostSetting { get; set; } = new();
    
    [Required] 
    public NaviSetting NaviSetting { get; set; } = new();
    
    [Required] 
    public GamepadSetting GamepadSetting { get; set; } = new();
    
    // Team Domain
    [Required]
    public TeamSetting TeamSetting { get; set; } = new();
    
    public ICollection<TagTeamData> TagTeamDatas { get; } = new List<TagTeamData>(); 
    public ICollection<OnlinePair> OnlinePairs { get; } = new List<OnlinePair>(); 
    
    // Title Domain
    [Required] 
    public UserTitleSetting UserTitleSetting { get; set; } = new();
    
    // Message Domain
    [Required] 
    public MessageSetting MessageSetting { get; set; } = new();

    [Required]
    public PrivateMatchRoomSetting PrivateMatchRoomSetting { get; set; } = new();
    
    // Upload Domain
    public ICollection<UploadImage> UploadImages { get; } = new List<UploadImage>(); 
    
    // Replay Domain
    public ICollection<UploadReplay> UploadReplays { get; } = new List<UploadReplay>(); 
    public ICollection<SharedUploadReplay> SharedUploadReplays { get; } = new List<SharedUploadReplay>();
    
    // Triad Ranking Domain
    public ICollection<TriadTargetDefeatedCount> TriadTargetDefeatedCounts { get; } = new List<TriadTargetDefeatedCount>();
    public ICollection<TriadWantedDefeatedCount> TriadWantedDefeatedCounts { get; } = new List<TriadWantedDefeatedCount>();
    public ICollection<TriadClearTime> TriadClearTimes { get; } = new List<TriadClearTime>();
    public ICollection<TriadHighScore> TriadHighScores { get; } = new List<TriadHighScore>();
    
    // Set Last Login Cabinet
    [Required]
    [StringLength(20)]
    public string LastLoginCabinet { get; set; } = string.Empty;
    
    // Battle History
    public ICollection<BattleHistory> BattleHistories { get; } = new List<BattleHistory>();
    
    public PreBattleHistory? PreBattleHistory { get; set; }
    public ChallengeMissionProfile? ChallengeMissionProfile { get; set; }
}