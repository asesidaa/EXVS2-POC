using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Cards;

namespace Server.Persistence.Configurations;

public class SnapshotConfigurations : IEntityTypeConfiguration<Snapshot>
{
    public void Configure(EntityTypeBuilder<Snapshot> builder)
    {
        builder.HasKey(x => x.Id);
    }
}