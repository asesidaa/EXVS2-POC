using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.Battle.History;
using WebUIOver.Shared.Dto.History.Stats;

namespace ServerOver.Mapper.Card.History;

[Mapper]
public static partial class MiscStatsMapper
{
    public static partial MiscStats ToMiscStats(this BattleHistory battleHistory);
}