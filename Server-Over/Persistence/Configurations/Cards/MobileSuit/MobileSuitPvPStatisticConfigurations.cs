using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.MobileSuit;

namespace ServerOver.Persistence.Configurations.Cards.MobileSuit;

public class MobileSuitPvPStatisticConfigurations : IEntityTypeConfiguration<MobileSuitPvPStatistic>
{
    public void Configure(EntityTypeBuilder<MobileSuitPvPStatistic> builder)
    {
        builder.HasKey(x => x.Id);
    }
}