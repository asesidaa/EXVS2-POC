using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Battle;

namespace ServerOver.Persistence.Configurations.Cards.Battle;

public class PlayerLevelConfigurations : IEntityTypeConfiguration<PlayerLevel>
{
    public void Configure(EntityTypeBuilder<PlayerLevel> builder)
    {
        builder.HasKey(x => x.Id);
    }
}