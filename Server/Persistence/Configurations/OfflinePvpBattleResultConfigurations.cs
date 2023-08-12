using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Cards;

namespace Server.Persistence.Configurations;

public class OfflinePvpBattleResultConfigurations : IEntityTypeConfiguration<OfflinePvpBattleResult>
{
    public void Configure(EntityTypeBuilder<OfflinePvpBattleResult> builder)
    {
        builder.HasKey(x => x.Id);
    }
}