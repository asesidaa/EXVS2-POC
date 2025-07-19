using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Battle;

namespace ServerOver.Persistence.Configurations.Cards.Battle;

public class PlayerBurstStatisticsConfiguration : IEntityTypeConfiguration<PlayerBurstStatistics>
{
    public void Configure(EntityTypeBuilder<PlayerBurstStatistics> builder)
    {
        builder.HasKey(x => x.Id);
    }
}