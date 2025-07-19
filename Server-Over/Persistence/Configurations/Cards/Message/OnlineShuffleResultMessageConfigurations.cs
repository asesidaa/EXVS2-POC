using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Message;

namespace ServerOver.Persistence.Configurations.Cards.Message;

public class OnlineShuffleResultMessageConfigurations : IEntityTypeConfiguration<OnlineShuffleResultMessage>
{
    public void Configure(EntityTypeBuilder<OnlineShuffleResultMessage> builder)
    {
        builder.HasKey(x => x.Id);
    }
}