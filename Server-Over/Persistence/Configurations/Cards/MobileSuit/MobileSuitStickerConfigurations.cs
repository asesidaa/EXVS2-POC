using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.MobileSuit;

namespace ServerOver.Persistence.Configurations.Cards.MobileSuit;

public class MobileSuitStickerConfigurations : IEntityTypeConfiguration<MobileSuitSticker>
{
    public void Configure(EntityTypeBuilder<MobileSuitSticker> builder)
    {
        builder.HasKey(x => x.StickerId);
    }
}