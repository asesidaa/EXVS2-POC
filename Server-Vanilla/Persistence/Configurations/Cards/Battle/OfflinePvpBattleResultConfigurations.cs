using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Battle;

namespace ServerVanilla.Persistence.Configurations.Cards.Battle;

public class OfflinePvpBattleResultConfigurations : IEntityTypeConfiguration<OfflinePvpBattleResult>
{
    public void Configure(EntityTypeBuilder<OfflinePvpBattleResult> builder)
    {
        builder.HasKey(x => x.Id);
    }
}