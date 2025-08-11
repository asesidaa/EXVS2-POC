using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Settings;

namespace ServerOver.Persistence.Configurations.Cards.Setting;

public class GamepadSettingConfigurations : IEntityTypeConfiguration<GamepadSetting>
{
    public void Configure(EntityTypeBuilder<GamepadSetting> builder)
    {
        builder.HasKey(x => x.Id);
    }
}