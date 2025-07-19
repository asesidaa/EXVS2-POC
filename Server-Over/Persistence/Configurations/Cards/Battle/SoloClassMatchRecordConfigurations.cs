using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Battle;

namespace ServerOver.Persistence.Configurations.Cards.Battle;

public class SoloClassMatchRecordConfigurations : IEntityTypeConfiguration<SoloClassMatchRecord>
{
    public void Configure(EntityTypeBuilder<SoloClassMatchRecord> builder)
    {
        builder.HasKey(x => x.Id);
    }
}