using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Cards;

namespace Server.Persistence.Configurations;

public class UploadReplayConfigurations : IEntityTypeConfiguration<UploadReplay>
{
    public void Configure(EntityTypeBuilder<UploadReplay> builder)
    {
        builder.HasKey(x => x.ReplayId);
    }
}