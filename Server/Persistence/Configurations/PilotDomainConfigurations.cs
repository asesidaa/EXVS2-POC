using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Server.Models.Cards;

namespace Server.Persistence.Configurations;

public class PilotDomainConfigurations : IEntityTypeConfiguration<PilotDomain>
{
    public void Configure(EntityTypeBuilder<PilotDomain> builder)
    {
        builder.HasKey(x => x.PilotId);
    }
}