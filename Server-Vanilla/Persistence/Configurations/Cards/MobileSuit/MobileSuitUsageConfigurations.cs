using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.MobileSuit;

namespace ServerVanilla.Persistence.Configurations.Cards.MobileSuit;

public class MobileSuitUsageConfigurations : IEntityTypeConfiguration<MobileSuitUsage>
{
    public void Configure(EntityTypeBuilder<MobileSuitUsage> builder)
    {
        builder.HasKey(x => x.Id);
    }
}