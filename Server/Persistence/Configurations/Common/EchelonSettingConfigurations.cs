using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Common;

namespace Server.Persistence.Configurations.Common;

public class EchelonSettingConfigurations : IEntityTypeConfiguration<EchelonSetting>
{
    public void Configure(EntityTypeBuilder<EchelonSetting> builder)
    {
        builder.HasKey(x => x.Id);
    }
}