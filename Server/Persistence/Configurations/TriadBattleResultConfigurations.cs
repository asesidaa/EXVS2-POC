using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Cards;

namespace Server.Persistence.Configurations;

public class TriadBattleResultConfigurations : IEntityTypeConfiguration<TriadBattleResult>
{
    public void Configure(EntityTypeBuilder<TriadBattleResult> builder)
    {
        builder.HasKey(x => x.Id);
    }
}