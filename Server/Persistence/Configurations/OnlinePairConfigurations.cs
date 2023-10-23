using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Cards;

namespace Server.Persistence.Configurations;

public class OnlinePairConfigurations : IEntityTypeConfiguration<OnlinePair>
{
    public void Configure(EntityTypeBuilder<OnlinePair> builder)
    {
        builder.HasKey(x => x.PairId);
    }
}