using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Profile;

namespace ServerOver.Persistence.Configurations.Cards.Profile;

public class DefaultStickerProfileConfigurations : IEntityTypeConfiguration<DefaultStickerProfile>
{
    public void Configure(EntityTypeBuilder<DefaultStickerProfile> builder)
    {
        builder.HasKey(x => x.Id);
    }
}