using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Message;

namespace ServerVanilla.Persistence.Configurations.Cards.Message;

public class OnlineResultMessageConfigurations : IEntityTypeConfiguration<OnlineResultMessage>
{
    public void Configure(EntityTypeBuilder<OnlineResultMessage> builder)
    {
        builder.HasKey(x => x.Id);
    }
}