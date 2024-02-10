using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.Common;

namespace ServerVanilla.Persistence.Configurations.Cards.Common;

public class EchelonSettingConfigurations : IEntityTypeConfiguration<EchelonSetting>
{
    public void Configure(EntityTypeBuilder<EchelonSetting> builder)
    {
        builder.HasKey(x => x.Id);
    }
}