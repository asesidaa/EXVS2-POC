using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Triad;

namespace ServerOver.Persistence.Configurations.Cards.Triad;

public class TriadCourseDataConfigurations : IEntityTypeConfiguration<TriadCourseData>
{
    public void Configure(EntityTypeBuilder<TriadCourseData> builder)
    {
        builder.HasKey(x => x.Id);
    }
}