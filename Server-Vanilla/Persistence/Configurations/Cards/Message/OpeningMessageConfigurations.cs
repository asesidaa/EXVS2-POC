using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Message;

namespace ServerVanilla.Persistence.Configurations.Cards.Message;

public class OpeningMessageConfigurations : IEntityTypeConfiguration<OpeningMessage>
{
    public void Configure(EntityTypeBuilder<OpeningMessage> builder)
    {
        builder.HasKey(x => x.Id);
    }
}