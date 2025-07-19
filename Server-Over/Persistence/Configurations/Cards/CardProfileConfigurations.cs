using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle;
using ServerOver.Models.Cards.Battle.History;
using ServerOver.Models.Cards.Mission;
using ServerOver.Models.Cards.Profile;
using ServerOver.Models.Cards.Settings;
using ServerOver.Models.Cards.Titles.User;
using ServerOver.Models.Cards.Triad;

namespace ServerOver.Persistence.Configurations.Cards;

public class CardProfileConfigurations : IEntityTypeConfiguration<CardProfile>
{
    public void Configure(EntityTypeBuilder<CardProfile> builder)
    {
        builder.HasKey(x => x.Id);
        
        // Battle Domain
        builder.HasOne(e => e.PlayerLevel)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<PlayerLevel>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.WinLossRecord)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<WinLossRecord>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.SoloClassMatchRecord)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<SoloClassMatchRecord>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.TeamClassMatchRecord)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<TeamClassMatchRecord>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.PlayerBattleStatistic)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<PlayerBattleStatistic>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.LicenseScoreRecord)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<LicenseScoreRecord>(e => e.CardId)
            .IsRequired();
        
        builder.HasMany(e => e.PlayerBurstStatistics)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        // Profile Domain
        builder.HasOne(e => e.PlayerProfile)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<PlayerProfile>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.CustomizeProfile)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<CustomizeProfile>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.TrainingProfile)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<TrainingProfile>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.DefaultStickerProfile)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<DefaultStickerProfile>(e => e.CardId)
            .IsRequired();
        
        // MS Domain
        builder.HasMany(e => e.MobileSuits)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        builder.HasMany(e => e.FavouriteMobileSuits)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        builder.HasMany(e => e.MobileSuitStickers)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        builder.HasMany(e => e.MobileSuitPvPStatistics)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        // Navi Domain
        builder.HasMany(e => e.Navis)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        // Triad Domain
        builder.HasOne(e => e.TriadMiscInfo)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<TriadMiscInfo>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.TriadPartner)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<TriadPartner>(e => e.CardId)
            .IsRequired();
        
        builder.HasMany(e => e.TriadCourseDatas)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        // Setting Domain
        builder.HasOne(e => e.GeneralSetting)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<GeneralSetting>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.BoostSetting)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<BoostSetting>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.NaviSetting)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<NaviSetting>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.GamepadSetting)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<GamepadSetting>(e => e.CardId)
            .IsRequired();
        
        // Title Domain
        builder.HasOne(e => e.UserTitleSetting)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<UserTitleSetting>(e => e.CardId)
            .IsRequired();
        
        // Message Domain
        builder.HasOne(e => e.MessageSetting)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<MessageSetting>(e => e.CardId)
            .IsRequired();
        
        // Team Domain
        builder.HasOne(e => e.TeamSetting)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<TeamSetting>(e => e.CardId)
            .IsRequired();
        
        builder.HasMany(e => e.OnlinePairs)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        builder.HasMany(e => e.TagTeamDatas)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        // Private Room Setting Domain
        builder.HasOne(e => e.PrivateMatchRoomSetting)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<PrivateMatchRoomSetting>(e => e.CardId)
            .IsRequired();
        
        // Upload Image
        builder.HasMany(e => e.UploadImages)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        // Replay Domain
        builder.HasMany(e => e.UploadReplays)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        builder.HasMany(e => e.SharedUploadReplays)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        // Triad Ranking Domain
        builder.HasMany(e => e.TriadTargetDefeatedCounts)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        builder.HasMany(e => e.TriadWantedDefeatedCounts)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        builder.HasMany(e => e.TriadClearTimes)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        builder.HasMany(e => e.TriadHighScores)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        // Battle Histories
        builder.HasMany(e => e.BattleHistories)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        builder.HasOne(e => e.PreBattleHistory)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<PreBattleHistory>(e => e.CardId)
            .IsRequired(false);
        
        // Challenge Mission
        builder.HasOne(e => e.ChallengeMissionProfile)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<ChallengeMissionProfile>(e => e.CardId)
            .IsRequired(false);
    }
}