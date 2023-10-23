using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Cards;

namespace Server.Persistence.Configurations;

public class TagTeamDataConfigurations : IEntityTypeConfiguration<TagTeamData>
{
    public void Configure(EntityTypeBuilder<TagTeamData> builder)
    {
        builder.HasKey(x => x.Id);
    }
}