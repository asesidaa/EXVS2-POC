using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ServerOver.Persistence.Configurations.Cards.Navi;

public class NaviConfigurations : IEntityTypeConfiguration<Models.Cards.Navi.Navi>
{
    public void Configure(EntityTypeBuilder<Models.Cards.Navi.Navi> builder)
    {
        builder.HasKey(x => x.Id);
    }
}