using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Replay;

namespace ServerVanilla.Persistence.Configurations.Cards.Replay;

public class UploadReplayConfigurations : IEntityTypeConfiguration<UploadReplay>
{
    public void Configure(EntityTypeBuilder<UploadReplay> builder)
    {
        builder.HasKey(x => x.ReplayId);
    }
}