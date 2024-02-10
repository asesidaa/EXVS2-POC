using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Message;

namespace ServerVanilla.Persistence.Configurations.Cards.Message;

public class PlayingMessageConfigurations : IEntityTypeConfiguration<PlayingMessage>
{
    public void Configure(EntityTypeBuilder<PlayingMessage> builder)
    {
        builder.HasKey(x => x.Id);
    }
}