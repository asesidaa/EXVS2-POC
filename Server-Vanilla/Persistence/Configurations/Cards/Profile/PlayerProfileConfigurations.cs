using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Profile;

namespace ServerVanilla.Persistence.Configurations.Cards.Profile;

public class PlayerProfileConfigurations : IEntityTypeConfiguration<PlayerProfile>
{
    public void Configure(EntityTypeBuilder<PlayerProfile> builder)
    {
        builder.HasKey(x => x.Id);
    }
}