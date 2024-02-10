using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Profile;

namespace ServerVanilla.Persistence.Configurations.Cards.Profile;

public class CustomizeProfileConfigurations : IEntityTypeConfiguration<CustomizeProfile>
{
    public void Configure(EntityTypeBuilder<CustomizeProfile> builder)
    {
        builder.HasKey(x => x.Id);
    }
}