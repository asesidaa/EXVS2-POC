using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Message;

namespace ServerOver.Persistence.Configurations.Cards.Message;

public class PlayingMessageConfigurations : IEntityTypeConfiguration<PlayingMessage>
{
    public void Configure(EntityTypeBuilder<PlayingMessage> builder)
    {
        builder.HasKey(x => x.Id);
    }
}