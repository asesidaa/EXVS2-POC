using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Settings;

namespace ServerOver.Persistence.Configurations.Cards.Setting;

public class TeamSettingConfigurations : IEntityTypeConfiguration<TeamSetting>
{
    public void Configure(EntityTypeBuilder<TeamSetting> builder)
    {
        builder.HasKey(x => x.Id);
    }
}