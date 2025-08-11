using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Battle;

namespace ServerOver.Persistence.Configurations.Cards.Battle;

public class LicenseScoreRecordConfigurations : IEntityTypeConfiguration<LicenseScoreRecord>
{
    public void Configure(EntityTypeBuilder<LicenseScoreRecord> builder)
    {
        builder.HasKey(x => x.Id);
    }
}