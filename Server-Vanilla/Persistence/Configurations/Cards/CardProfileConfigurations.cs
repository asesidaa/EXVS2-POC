using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Models.Cards;
using ServerVanilla.Models.Cards.Message;
using ServerVanilla.Models.Cards.Profile;
using ServerVanilla.Models.Cards.Settings;
using ServerVanilla.Models.Cards.Title;
using ServerVanilla.Models.Cards.Triad;

namespace ServerVanilla.Persistence.Configurations.Cards;

public class CardProfileConfigurations : IEntityTypeConfiguration<CardProfile>
{
    public void Configure(EntityTypeBuilder<CardProfile> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(e => e.PlayerProfile)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<PlayerProfile>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.BattleProfile)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<BattleProfile>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.CustomizeProfile)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<CustomizeProfile>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.TrainingProfile)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<TrainingProfile>(e => e.CardId)
            .IsRequired();
        
        builder.HasMany(e => e.MobileSuits)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        builder.HasMany(e => e.FavouriteMobileSuits)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        builder.HasMany(e => e.Navi)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
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
        
        builder.HasMany(e => e.TagTeamDatas)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        builder.HasOne(e => e.DefaultTitle)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<DefaultTitle>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.OpeningMessage)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<OpeningMessage>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.PlayingMessage)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<PlayingMessage>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.ResultMessage)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<ResultMessage>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.OnlineOpeningMessage)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<OnlineOpeningMessage>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.OnlinePlayingMessage)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<OnlinePlayingMessage>(e => e.CardId)
            .IsRequired();
        
        builder.HasOne(e => e.OnlineResultMessage)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<OnlineResultMessage>(e => e.CardId)
            .IsRequired();
        
        builder.HasMany(e => e.UploadReplays)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        builder.HasMany(e => e.SharedUploadReplays)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        
        builder.HasMany(e => e.OfflinePvpBattleResults)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
    }
}