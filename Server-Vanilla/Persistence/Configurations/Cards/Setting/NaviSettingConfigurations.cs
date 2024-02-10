using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Settings;

namespace ServerVanilla.Persistence.Configurations.Cards.Setting;

public class NaviSettingConfigurations : IEntityTypeConfiguration<NaviSetting>
{
    public void Configure(EntityTypeBuilder<NaviSetting> builder)
    {
        builder.HasKey(x => x.Id);
    }
}