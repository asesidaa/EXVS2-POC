using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Settings;

namespace ServerOver.Persistence.Configurations.Cards.Setting;

public class BoostSettingConfigurations : IEntityTypeConfiguration<BoostSetting>
{
    public void Configure(EntityTypeBuilder<BoostSetting> builder)
    {
        builder.HasKey(x => x.Id);
    }
}