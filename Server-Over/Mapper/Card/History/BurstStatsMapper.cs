using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.Battle.History;
using WebUIOver.Shared.Dto.History.Stats;

namespace ServerOver.Mapper.Card.History;

[Mapper]
public static partial class BurstStatsMapper
{
    public static partial BurstStats ToBurstStats(this BattleHistory battleHistory);
}