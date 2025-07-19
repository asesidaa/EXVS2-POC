using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Titles.MobileSuit;

namespace ServerOver.Persistence.Configurations.Cards.Titles.MobileSuit;

public class MobileSuitClassMatchTitleConfigurations : IEntityTypeConfiguration<MobileSuitClassMatchTitle>
{
    public void Configure(EntityTypeBuilder<MobileSuitClassMatchTitle> builder)
    {
        builder.HasKey(x => x.Id);
    }
}