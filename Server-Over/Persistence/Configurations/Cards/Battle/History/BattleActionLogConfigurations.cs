using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Battle.History;

namespace ServerOver.Persistence.Configurations.Cards.Battle.History;

public class BattleActionLogConfigurations : IEntityTypeConfiguration<BattleActionLog>
{
    public void Configure(EntityTypeBuilder<BattleActionLog> builder)
    {
        builder.HasKey(x => x.Id);
    }
}