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
        // builder.OwnsOne(x => x.PilotDomain);
        // builder.OwnsOne(x => x.UserDomain);
    }
}