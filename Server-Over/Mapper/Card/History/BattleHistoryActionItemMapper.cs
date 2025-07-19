using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.Battle.History;
using WebUIOver.Shared.Dto.History;

namespace ServerOver.Mapper.Card.History;

[Mapper]
public static partial class BattleHistoryActionItemMapper
{
    public static partial BattleHistoryActionItem ToBattleHistoryActionItem(this BattleActionLog battleActionLog);
}