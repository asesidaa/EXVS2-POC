using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Battle.History;

namespace ServerOver.Persistence.Configurations.Cards.Battle.History;

public class BattleHistoryConfigurations : IEntityTypeConfiguration<BattleHistory>
{
    public void Configure(EntityTypeBuilder<BattleHistory> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(e => e.BattleSelf)
            .WithOne(e => e.BattleHistory)
            .HasForeignKey<BattleSelf>(e => e.BattleHistoryId)
            .IsRequired();
        
        builder.HasOne(e => e.Ally)
            .WithOne(e => e.BattleHistory)
            .HasForeignKey<BattleAlly>(e => e.BattleHistoryId)
            .IsRequired(false);
        
        builder.HasMany(e => e.Targets)
            .WithOne(e => e.BattleHistory)
            .HasForeignKey(e => e.BattleHistoryId)
            .IsRequired(false);
        
        builder.HasMany(e => e.ActionLogs)
            .WithOne(e => e.BattleHistory)
            .HasForeignKey(e => e.BattleHistoryId)
            .IsRequired(false);
    }
}