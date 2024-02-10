using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Message;

namespace ServerVanilla.Persistence.Configurations.Cards.Message;

public class OnlineOpeningMessageConfigurations : IEntityTypeConfiguration<OnlineOpeningMessage>
{
    public void Configure(EntityTypeBuilder<OnlineOpeningMessage> builder)
    {
        builder.HasKey(x => x.Id);
    }
}