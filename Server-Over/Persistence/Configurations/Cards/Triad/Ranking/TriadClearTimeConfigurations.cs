using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Triad.Ranking;

namespace ServerOver.Persistence.Configurations.Cards.Triad.Ranking;

public class TriadClearTimeConfigurations : IEntityTypeConfiguration<TriadClearTime>
{
    public void Configure(EntityTypeBuilder<TriadClearTime> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.CardId, x.Year, x.Month })
            .IsUnique();
    }
}