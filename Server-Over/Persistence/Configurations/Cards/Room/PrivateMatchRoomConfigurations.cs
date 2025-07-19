using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ServerOver.Models.Cards.Room;

namespace ServerOver.Persistence.Configurations.Cards.Room;

public class PrivateMatchRoomConfigurations : IEntityTypeConfiguration<PrivateMatchRoom>
{
    public void Configure(EntityTypeBuilder<PrivateMatchRoom> builder)
    {
        builder.HasKey(x => x.Id);
    }
}