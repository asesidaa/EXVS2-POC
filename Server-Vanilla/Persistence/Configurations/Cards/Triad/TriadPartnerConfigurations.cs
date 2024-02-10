using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Triad;

namespace ServerVanilla.Persistence.Configurations.Cards.Triad;

public class TriadPartnerConfigurations : IEntityTypeConfiguration<TriadPartner>
{
    public void Configure(EntityTypeBuilder<TriadPartner> builder)
    {
        builder.HasKey(x => x.Id);
    }
}