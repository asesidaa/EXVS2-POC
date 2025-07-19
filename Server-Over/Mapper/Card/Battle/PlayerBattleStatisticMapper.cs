using Riok.Mapperly.Abstractions;
using ServerOver.Context.Tracker;
using ServerOver.Models.Cards.Battle;

namespace ServerOver.Mapper.Card.Battle;

[Mapper]
public static partial class PlayerBattleStatisticMapper
{
    public static partial PilotTrackerContext ToPilotTrackerContext(this PlayerBattleStatistic playerBattleStatistic);
}