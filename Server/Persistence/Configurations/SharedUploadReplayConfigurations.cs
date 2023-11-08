using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Cards;

namespace Server.Persistence.Configurations;

public class SharedUploadReplayConfigurations : IEntityTypeConfiguration<SharedUploadReplay>
{
    public void Configure(EntityTypeBuilder<SharedUploadReplay> builder)
    {
        builder.HasKey(x => x.ReplayId);
    }
}