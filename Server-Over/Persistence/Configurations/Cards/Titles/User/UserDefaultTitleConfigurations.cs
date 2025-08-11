using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Titles.User;

namespace ServerOver.Persistence.Configurations.Cards.Titles.User;

public class UserDefaultTitleConfigurations : IEntityTypeConfiguration<UserDefaultTitle>
{
    public void Configure(EntityTypeBuilder<UserDefaultTitle> builder)
    {
        builder.HasKey(x => x.Id);
    }
}