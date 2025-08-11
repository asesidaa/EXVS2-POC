using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Room;
using ServerOver.Models.Cards.Settings;

namespace ServerOver.Persistence.Configurations.Cards.Setting;

public class PrivateMatchRoomSettingConfigurations : IEntityTypeConfiguration<PrivateMatchRoomSetting>
{
    public void Configure(EntityTypeBuilder<PrivateMatchRoomSetting> builder)
    {
        builder.HasKey(x => x.PrivateMatchRoomSettingId);
        
        builder.HasOne(e => e.SelfPrivateRoomConfig)
            .WithOne(e => e.PrivateMatchRoomSetting)
            .HasForeignKey<PrivateMatchRoom>(e => e.PrivateMatchRoomSettingId)
            .IsRequired();
    }
}