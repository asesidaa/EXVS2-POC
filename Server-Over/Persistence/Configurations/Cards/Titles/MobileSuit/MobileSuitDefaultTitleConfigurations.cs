using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Titles.MobileSuit;

namespace ServerOver.Persistence.Configurations.Cards.Titles.MobileSuit;

public class MobileSuitDefaultTitleConfigurations : IEntityTypeConfiguration<MobileSuitDefaultTitle>
{
    public void Configure(EntityTypeBuilder<MobileSuitDefaultTitle> builder)
    {
        builder.HasKey(x => x.Id);
    }
}