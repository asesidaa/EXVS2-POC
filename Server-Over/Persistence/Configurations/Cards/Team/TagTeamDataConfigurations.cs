using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Team;

namespace ServerOver.Persistence.Configurations.Cards.Team;

public class TagTeamDataConfigurations : IEntityTypeConfiguration<TagTeamData>
{
    public void Configure(EntityTypeBuilder<TagTeamData> builder)
    {
        builder.HasKey(x => x.Id);
    }
}