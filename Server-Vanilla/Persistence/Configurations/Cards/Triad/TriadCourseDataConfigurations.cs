using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Triad;

namespace ServerVanilla.Persistence.Configurations.Cards.Triad;

public class TriadCourseDataConfigurations : IEntityTypeConfiguration<TriadCourseData>
{
    public void Configure(EntityTypeBuilder<TriadCourseData> builder)
    {
        builder.HasKey(x => x.Id);
    }
}