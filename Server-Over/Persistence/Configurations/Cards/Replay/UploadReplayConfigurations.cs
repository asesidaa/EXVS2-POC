using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Replay;

namespace ServerOver.Persistence.Configurations.Cards.Replay;

public class UploadReplayConfigurations : IEntityTypeConfiguration<UploadReplay>
{
    public void Configure(EntityTypeBuilder<UploadReplay> builder)
    {
        builder.HasKey(x => x.ReplayId);
    }
}