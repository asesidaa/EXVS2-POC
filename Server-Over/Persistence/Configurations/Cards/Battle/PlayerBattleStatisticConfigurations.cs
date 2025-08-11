using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Battle;

namespace ServerOver.Persistence.Configurations.Cards.Battle;

public class PlayerBattleStatisticConfigurations : IEntityTypeConfiguration<PlayerBattleStatistic>
{
    public void Configure(EntityTypeBuilder<PlayerBattleStatistic> builder)
    {
        builder.HasKey(x => x.Id);
    }
}