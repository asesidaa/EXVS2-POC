using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Titles.MobileSuit;

namespace ServerOver.Persistence.Configurations.Cards.Titles.MobileSuit;

public class MobileSuitTriadTitleConfigurations : IEntityTypeConfiguration<MobileSuitTriadTitle>
{
    public void Configure(EntityTypeBuilder<MobileSuitTriadTitle> builder)
    {
        builder.HasKey(x => x.Id);
    }
}