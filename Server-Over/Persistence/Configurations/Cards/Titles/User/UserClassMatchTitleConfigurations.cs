using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Titles.User;

namespace ServerOver.Persistence.Configurations.Cards.Titles.User;

public class UserClassMatchTitleConfigurations : IEntityTypeConfiguration<UserClassMatchTitle>
{
    public void Configure(EntityTypeBuilder<UserClassMatchTitle> builder)
    {
        builder.HasKey(x => x.Id);
    }
}