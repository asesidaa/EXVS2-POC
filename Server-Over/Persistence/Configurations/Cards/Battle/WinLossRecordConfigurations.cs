using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Battle;

namespace ServerOver.Persistence.Configurations.Cards.Battle;

public class WinLossRecordConfigurations : IEntityTypeConfiguration<WinLossRecord>
{
    public void Configure(EntityTypeBuilder<WinLossRecord> builder)
    {
        builder.HasKey(x => x.Id);
    }
}