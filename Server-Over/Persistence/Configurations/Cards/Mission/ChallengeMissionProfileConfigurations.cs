using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Mission;

namespace ServerOver.Persistence.Configurations.Cards.Mission;

public class ChallengeMissionProfileConfigurations : IEntityTypeConfiguration<ChallengeMissionProfile>
{
    public void Configure(EntityTypeBuilder<ChallengeMissionProfile> builder)
    {
        builder.HasKey(x => x.Id);
    }
}