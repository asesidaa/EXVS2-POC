using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Battle.History;

namespace ServerOver.Persistence.Configurations.Cards.Battle.History;

public class BattleSelfConfigurations : IEntityTypeConfiguration<BattleSelf>
{
    public void Configure(EntityTypeBuilder<BattleSelf> builder)
    {
        builder.HasKey(x => x.Id);
    }
}