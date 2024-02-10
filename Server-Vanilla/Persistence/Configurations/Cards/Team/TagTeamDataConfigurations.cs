using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Team;

namespace ServerVanilla.Persistence.Configurations.Cards.Team;

public class TagTeamDataConfigurations : IEntityTypeConfiguration<TagTeamData>
{
    public void Configure(EntityTypeBuilder<TagTeamData> builder)
    {
        builder.HasKey(x => x.Id);
    }
}