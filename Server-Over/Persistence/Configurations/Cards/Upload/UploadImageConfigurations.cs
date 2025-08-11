using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Upload;

namespace ServerOver.Persistence.Configurations.Cards.Upload;

public class UploadImageConfigurations : IEntityTypeConfiguration<UploadImage>
{
    public void Configure(EntityTypeBuilder<UploadImage> builder)
    {
        builder.HasKey(x => x.ImageId);
    }
}