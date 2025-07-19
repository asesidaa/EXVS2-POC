using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Team;

namespace ServerOver.Persistence.Configurations.Cards.Team;

public class OnlinePairConfigurations : IEntityTypeConfiguration<OnlinePair>
{
    public void Configure(EntityTypeBuilder<OnlinePair> builder)
    {
        builder.HasKey(x => x.PairId);
    }
}