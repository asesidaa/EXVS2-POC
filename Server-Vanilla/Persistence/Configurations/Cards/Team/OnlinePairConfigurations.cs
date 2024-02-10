using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Team;

namespace ServerVanilla.Persistence.Configurations.Cards.Team;

public class OnlinePairConfigurations : IEntityTypeConfiguration<OnlinePair>
{
    public void Configure(EntityTypeBuilder<OnlinePair> builder)
    {
        builder.HasKey(x => x.PairId);
    }
}