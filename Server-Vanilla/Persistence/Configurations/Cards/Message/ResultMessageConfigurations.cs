using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Message;

namespace ServerVanilla.Persistence.Configurations.Cards.Message;

public class ResultMessageConfigurations : IEntityTypeConfiguration<ResultMessage>
{
    public void Configure(EntityTypeBuilder<ResultMessage> builder)
    {
        builder.HasKey(x => x.Id);
    }
}