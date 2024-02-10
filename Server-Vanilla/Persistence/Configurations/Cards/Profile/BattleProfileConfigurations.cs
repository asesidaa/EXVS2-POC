using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Profile;

namespace ServerVanilla.Persistence.Configurations.Cards.Profile;

public class BattleProfileConfigurations : IEntityTypeConfiguration<BattleProfile>
{
    public void Configure(EntityTypeBuilder<BattleProfile> builder)
    {
        builder.HasKey(x => x.Id);
    }
}