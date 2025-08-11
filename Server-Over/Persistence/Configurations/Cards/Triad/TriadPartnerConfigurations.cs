using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Triad;

namespace ServerOver.Persistence.Configurations.Cards.Triad;

public class TriadPartnerConfigurations : IEntityTypeConfiguration<TriadPartner>
{
    public void Configure(EntityTypeBuilder<TriadPartner> builder)
    {
        builder.HasKey(x => x.Id);
    }
}