using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Message;
using ServerOver.Models.Cards.Settings;

namespace ServerOver.Persistence.Configurations.Cards.Setting;

public class MessageSettingConfigurations : IEntityTypeConfiguration<MessageSetting>
{
    public void Configure(EntityTypeBuilder<MessageSetting> builder)
    {
        builder.HasKey(x => x.MessageSettingId);
        
        builder.HasOne(e => e.OpeningMessage)
            .WithOne(e => e.MessageSetting)
            .HasForeignKey<OpeningMessage>(e => e.MessageSettingId)
            .IsRequired();
        
        builder.HasOne(e => e.PlayingMessage)
            .WithOne(e => e.MessageSetting)
            .HasForeignKey<PlayingMessage>(e => e.MessageSettingId)
            .IsRequired();
        
        builder.HasOne(e => e.ResultMessage)
            .WithOne(e => e.MessageSetting)
            .HasForeignKey<ResultMessage>(e => e.MessageSettingId)
            .IsRequired();
        
        builder.HasOne(e => e.OnlineShuffleOpeningMessage)
            .WithOne(e => e.MessageSetting)
            .HasForeignKey<OnlineShuffleOpeningMessage>(e => e.MessageSettingId)
            .IsRequired();
        
        builder.HasOne(e => e.OnlineShufflePlayingMessage)
            .WithOne(e => e.MessageSetting)
            .HasForeignKey<OnlineShufflePlayingMessage>(e => e.MessageSettingId)
            .IsRequired();
        
        builder.HasOne(e => e.OnlineShuffleResultMessage)
            .WithOne(e => e.MessageSetting)
            .HasForeignKey<OnlineShuffleResultMessage>(e => e.MessageSettingId)
            .IsRequired();
    }
}