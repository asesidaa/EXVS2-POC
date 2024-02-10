using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Title;

namespace ServerVanilla.Persistence.Configurations.Cards.Title;

public class DefaultTitleConfigurations : IEntityTypeConfiguration<DefaultTitle>
{
    public void Configure(EntityTypeBuilder<DefaultTitle> builder)
    {
        builder.HasKey(x => x.Id);
    }
}