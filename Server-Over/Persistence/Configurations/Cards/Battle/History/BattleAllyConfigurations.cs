using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Battle.History;

namespace ServerOver.Persistence.Configurations.Cards.Battle.History;

public class BattleAllyConfigurations : IEntityTypeConfiguration<BattleAlly>
{
    public void Configure(EntityTypeBuilder<BattleAlly> builder)
    {
        builder.HasKey(x => x.Id);
    }
}