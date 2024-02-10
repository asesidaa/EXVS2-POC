using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Message;

namespace ServerVanilla.Persistence.Configurations.Cards.Message;

public class OnlinePlayingMessageConfigurations : IEntityTypeConfiguration<OnlinePlayingMessage>
{
    public void Configure(EntityTypeBuilder<OnlinePlayingMessage> builder)
    {
        builder.HasKey(x => x.Id);
    }
}