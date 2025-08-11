using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.Battle.History;
using WebUIOver.Shared.Dto.History.Stats;

namespace ServerOver.Mapper.Card.History;

[Mapper]
public static partial class DamageStatsMapper
{
    public static partial DamageStats ToDamageStats(this BattleHistory battleHistory);
}