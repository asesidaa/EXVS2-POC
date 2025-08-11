using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ServerOver.Models;
using ServerOver.Models.Boot;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle;
using ServerOver.Models.Cards.Battle.History;
using ServerOver.Models.Cards.Message;
using ServerOver.Models.Cards.Mission;
using ServerOver.Models.Cards.MobileSuit;
using ServerOver.Models.Cards.Navi;
using ServerOver.Models.Cards.Profile;
using ServerOver.Models.Cards.Room;
using ServerOver.Models.Cards.Settings;
using ServerOver.Models.Cards.Team;
using ServerOver.Models.Cards.Titles.User;
using ServerOver.Models.Cards.Triad;
using ServerOver.Models.Cards.Triad.Ranking;
using ServerOver.Views.Battle;
using ServerOver.Views.ClassMatch.Percentile;
using ServerOver.Views.Triad;
using ServerOver.Views.Usage;

namespace ServerOver.Persistence
{
    public class ServerDbContext : DbContext
    {
        public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options) 
        { 
            
        }
        
        public DbSet<InvalidVisit> InvalidVisits { get; set; }
        
        // For Card
        public DbSet<CardProfile> CardProfiles { get; set; }
        
        // Profile Domain
        public DbSet<PlayerProfile> PlayerProfileDbSet { get; set; }
        public DbSet<TrainingProfile> TrainingProfileDbSet { get; set; }
        public DbSet<CustomizeProfile> CustomizeProfileDbSet { get; set; }
        public DbSet<DefaultStickerProfile> DefaultStickerProfileDbSet { get; set; }
        
        // Title Domain
        public DbSet<UserTitleSetting> UserTitleSettingDbSet { get; set; }
        
        public DbSet<PlayerLevel> PlayerLevelDbSet { get; set; }
        public DbSet<Navi> NaviDbSet { get; set; }
        public DbSet<MobileSuitUsage> MobileSuitUsageDbSet { get; set; }
        public DbSet<FavouriteMobileSuit> FavouriteMobileSuitDbSet { get; set; }
        public DbSet<SoloClassMatchRecord> SoloClassMatchRecordDbSet { get; set; }
        public DbSet<TeamClassMatchRecord> TeamClassMatchRecordDbSet { get; set; }
        public DbSet<WinLossRecord> WinLossRecordDbSet { get; set; }
        public DbSet<LicenseScoreRecord> LicenseScoreRecordDbSet { get; set; }
        public DbSet<PlayerBurstStatistics> PlayerBurstStatisticsDbSet { get; set; }
        
        // Triad Domain
        public DbSet<TriadMiscInfo> TriadMiscInfoDbSet { get; set; }
        public DbSet<TriadPartner> TriadPartnerDbSet { get; set; }
        public DbSet<TriadCourseData> TriadCourseDataDbSet { get; set; }
        
        // Setting Domain
        public DbSet<NaviSetting> NaviSettingDbSet { get; set; }
        public DbSet<BoostSetting> BoostSettingDbSet { get; set; }
        public DbSet<GeneralSetting> GeneralSettingDbSet { get; set; }
        public DbSet<GamepadSetting> GamepadSettingDbSet { get; set; }
        public DbSet<TeamSetting> TeamSettingDbSet { get; set; }
        
        // Message Domain
        public DbSet<MessageSetting> MessageSettingDbSet { get; set; }
        public DbSet<OpeningMessage> OpeningMessageDbSet { get; set; }
        public DbSet<PlayingMessage> PlayingMessageDbSet { get; set; }
        public DbSet<ResultMessage> ResultMessageDbSet { get; set; }
        public DbSet<OnlineShuffleOpeningMessage> OnlineShuffleOpeningMessageDbSet { get; set; }
        public DbSet<OnlineShufflePlayingMessage> OnlineShufflePlayingMessageDbSet { get; set; }
        public DbSet<OnlineShuffleResultMessage> OnlineShuffleResultMessageDbSet { get; set; }
        
        // Sticker and Tracker Domain
        public DbSet<MobileSuitSticker> MobileSuitStickerDbSet { get; set; }
        public DbSet<PlayerBattleStatistic> PlayerBattleStatisticDbSet { get; set; }
        public DbSet<MobileSuitPvPStatistic> MobileSuitPvPStatisticDbSet { get; set; }
        
        // Team Domain
        public DbSet<TagTeamData> TagTeamDataDbSet { get; set; }
        public DbSet<OnlinePair> OnlinePairDbSet { get; set; }
        
        // Private Room Domain
        public DbSet<PrivateMatchRoomSetting> PrivateMatchRoomSettingDbSet { get; set; }
        public DbSet<PrivateMatchRoom> PrivateMatchRoomDbSet { get; set; }
        
        // Triad Ranking Domain
        public DbSet<TriadTargetDefeatedCount> TriadTargetDefeatedCountDbSet { get; set; }
        public DbSet<TriadWantedDefeatedCount> TriadWantedDefeatedCountDbSet { get; set; }
        public DbSet<TriadClearTime> TriadClearTimeDbSet { get; set; }
        public DbSet<TriadHighScore> TriadHighScoreDbSet { get; set; }
        
        // Battle History View
        public DbSet<BattleHistory> BattleHistoryDbSet { get; set; }
        public DbSet<BattleSelf> BattleSelfDbSet { get; set; }
        public DbSet<BattleAlly> BattleAllyDbSet { get; set; }
        public DbSet<BattleTarget> BattleTargetDbSet { get; set; }
        public DbSet<BattleActionLog> BattleActionLogsDbSet { get; set; }
        public DbSet<PreBattleHistory> PreBattleHistoryDbSet { get; set; }
        
        // Challenge Profile
        public DbSet<ChallengeMissionProfile> ChallengeMissionProfileDbSet { get; set; }
        
        
        // Online Class Match Percentile Views
        public DbSet<SoloPilotPercentileView> SoloPilotPercentileViews { get; set; }
        public DbSet<SoloValiantPercentileView> SoloValiantPercentileViews { get; set; }
        public DbSet<SoloAcePercentileView> SoloAcePercentileViews { get; set; }
        public DbSet<SoloOverPercentileView> SoloOverPercentileViews { get; set; }
        public DbSet<TeamPilotPercentileView> TeamPilotPercentileViews { get; set; }
        public DbSet<TeamValiantPercentileView> TeamValiantPercentileViews { get; set; }
        public DbSet<TeamAcePercentileView> TeamAcePercentileViews { get; set; }
        public DbSet<TeamOverPercentileView> TeamOverPercentileViews { get; set; }
        public DbSet<SoloOverRankView> SoloOverRankViews { get; set; }
        public DbSet<TeamOverRankView> TeamOverRankViews { get; set; }
        
        // Triad Rank Views
        public DbSet<TargetDefeatedCountView> TargetDefeatedCountViews { get; set; }
        public DbSet<WantedDefeatedCountView> WantedDefeatedCountViews { get; set; }
        public DbSet<ClearTimeView> ClearTimeViews { get; set; }
        public DbSet<HighScoreView> HighScoreViews { get; set; }
        
        // Player Level Rank View
        public DbSet<PlayerLevelRankView> PlayerLevelRankViews { get; set; }
        
        // Mobile Suit Usage View
        public DbSet<MobileSuitUsageView> MobileSuitUsageViews { get; set; }
        
        // Burst Usage View
        public DbSet<BurstUsageView> BurstUsageViews { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            modelBuilder
                .Entity<CardProfile>()
                .HasMany(e => e.FavouriteMobileSuits)
                .WithOne(e => e.CardProfile)
                .OnDelete(DeleteBehavior.ClientCascade);
            
            modelBuilder
                .Entity<CardProfile>()
                .HasMany(e => e.OnlinePairs)
                .WithOne(e => e.CardProfile)
                .OnDelete(DeleteBehavior.ClientCascade);
            
            modelBuilder
                .Entity<SoloPilotPercentileView>()
                .ToView("exvs2ob_solo_pilot_percentile_view");
            
            modelBuilder
                .Entity<SoloValiantPercentileView>()
                .ToView("exvs2ob_solo_valiant_percentile_view");
            
            modelBuilder
                .Entity<SoloAcePercentileView>()
                .ToView("exvs2ob_solo_ace_percentile_view");
            
            modelBuilder
                .Entity<SoloOverPercentileView>()
                .ToView("exvs2ob_solo_over_percentile_view");
            
            modelBuilder
                .Entity<TeamPilotPercentileView>()
                .ToView("exvs2ob_team_pilot_percentile_view");
            
            modelBuilder
                .Entity<TeamValiantPercentileView>()
                .ToView("exvs2ob_team_valiant_percentile_view");
            
            modelBuilder
                .Entity<TeamAcePercentileView>()
                .ToView("exvs2ob_team_ace_percentile_view");
            
            modelBuilder
                .Entity<TeamOverPercentileView>()
                .ToView("exvs2ob_team_over_percentile_view");
            
            modelBuilder
                .Entity<SoloOverRankView>()
                .ToView("exvs2ob_solo_over_rank_view");
            
            modelBuilder
                .Entity<TeamOverRankView>()
                .ToView("exvs2ob_team_over_rank_view");
            
            modelBuilder
                .Entity<TargetDefeatedCountView>()
                .ToView("exvs2ob_triad_target_defeated_count_view");
            
            modelBuilder
                .Entity<WantedDefeatedCountView>()
                .ToView("exvs2ob_triad_wanted_defeated_count_view");
            
            modelBuilder
                .Entity<ClearTimeView>()
                .ToView("exvs2ob_triad_clear_time_view");
            
            modelBuilder
                .Entity<HighScoreView>()
                .ToView("exvs2ob_triad_high_score_view");
            
            modelBuilder
                .Entity<PlayerLevelRankView>()
                .ToView("exvs2ob_player_level_rank_view");
            
            modelBuilder
                .Entity<MobileSuitUsageView>()
                .ToView("exvs2ob_mobile_suit_usage_view");
            
            modelBuilder
                .Entity<BurstUsageView>()
                .ToView("exvs2ob_burst_usage_view");
        }

        public override int SaveChanges()
        {
            var entityEntries = ChangeTracker.Entries().ToList();

            entityEntries.ForEach(entityEntry =>
            {
                if (entityEntry.State == EntityState.Added)
                {
                    Entry(entityEntry.Entity).Property(nameof(BaseEntity.CreateTime)).CurrentValue = DateTime.Now;
                    Entry(entityEntry.Entity).Property(nameof(BaseEntity.UpdateTime)).CurrentValue = DateTime.Now;
                }
                if (entityEntry.State == EntityState.Modified)
                {
                    Entry(entityEntry.Entity).Property(nameof(BaseEntity.UpdateTime)).CurrentValue = DateTime.Now;
                }
            });
            
            return base.SaveChanges();
        }
    }
}