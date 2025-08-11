using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Battle.History;

namespace ServerOver.Persistence.Configurations.Cards.Battle.History;

public class BattleTargetConfigurations : IEntityTypeConfiguration<BattleTarget>
{
    public void Configure(EntityTypeBuilder<BattleTarget> builder)
    {
        builder.HasKey(x => x.Id);
    }
}