using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Cards;

namespace Server.Persistence.Configurations;

public class UploadImageConfigurations : IEntityTypeConfiguration<UploadImage>
{
    public void Configure(EntityTypeBuilder<UploadImage> builder)
    {
        builder.HasKey(x => x.ImageId);
    }
}