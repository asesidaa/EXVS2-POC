using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Titles.User;

namespace ServerOver.Persistence.Configurations.Cards.Titles.User;

public class UserTitleSettingConfigurations : IEntityTypeConfiguration<UserTitleSetting>
{
    public void Configure(EntityTypeBuilder<UserTitleSetting> builder)
    {
        builder.HasKey(x => x.UserTitleSettingId);
        
        builder.HasOne(e => e.UserDefaultTitle)
            .WithOne(e => e.UserTitleSetting)
            .HasForeignKey<UserDefaultTitle>(e => e.UserTitleSettingId)
            .IsRequired();
        
        builder.HasOne(e => e.UserTriadTitle)
            .WithOne(e => e.UserTitleSetting)
            .HasForeignKey<UserTriadTitle>(e => e.UserTitleSettingId)
            .IsRequired();
        
        builder.HasOne(e => e.UserClassMatchTitle)
            .WithOne(e => e.UserTitleSetting)
            .HasForeignKey<UserClassMatchTitle>(e => e.UserTitleSettingId)
            .IsRequired();
    }
}