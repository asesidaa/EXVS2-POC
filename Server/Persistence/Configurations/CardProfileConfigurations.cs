using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Server.Models.Cards;

namespace Server.Persistence.Configurations;

public class CardProfileConfigurations : IEntityTypeConfiguration<CardProfile>
{
    public void Configure(EntityTypeBuilder<CardProfile> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(e => e.PilotDomain)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<PilotDomain>(e => e.CardId)
            .IsRequired();
        builder.HasOne(e => e.UserDomain)
            .WithOne(e => e.CardProfile)
            .HasForeignKey<UserDomain>(e => e.CardId)
            .IsRequired();
        builder.HasMany(e => e.UploadImages)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        builder.HasMany(e => e.OnlinePairs)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        builder.HasMany(e => e.TriadBattleResults)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
        builder.HasMany(e => e.OfflinePvpBattleResults)
            .WithOne(e => e.CardProfile)
            .HasForeignKey(e => e.CardId)
            .IsRequired(false);
    }
}