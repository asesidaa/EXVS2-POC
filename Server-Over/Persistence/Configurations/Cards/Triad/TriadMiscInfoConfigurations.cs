using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Triad;

namespace ServerOver.Persistence.Configurations.Cards.Triad;

public class TriadMiscInfoConfigurations : IEntityTypeConfiguration<TriadMiscInfo>
{
    public void Configure(EntityTypeBuilder<TriadMiscInfo> builder)
    {
        builder.HasKey(x => x.Id);
    }
}