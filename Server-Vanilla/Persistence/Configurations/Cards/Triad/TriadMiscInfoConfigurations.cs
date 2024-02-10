using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Triad;

namespace ServerVanilla.Persistence.Configurations.Cards.Triad;

public class TriadMiscInfoConfigurations : IEntityTypeConfiguration<TriadMiscInfo>
{
    public void Configure(EntityTypeBuilder<TriadMiscInfo> builder)
    {
        builder.HasKey(x => x.Id);
    }
}