using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Message;

namespace ServerOver.Persistence.Configurations.Cards.Message;

public class ResultMessageConfigurations : IEntityTypeConfiguration<ResultMessage>
{
    public void Configure(EntityTypeBuilder<ResultMessage> builder)
    {
        builder.HasKey(x => x.Id);
    }
}