using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Message;

namespace ServerOver.Persistence.Configurations.Cards.Message;

public class OnlineShuffleOpeningMessageConfigurations : IEntityTypeConfiguration<OnlineShuffleOpeningMessage>
{
    public void Configure(EntityTypeBuilder<OnlineShuffleOpeningMessage> builder)
    {
        builder.HasKey(x => x.Id);
    }
}