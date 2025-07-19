using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.Battle.History;
using WebUIOver.Shared.Dto.History;

namespace ServerOver.Mapper.Card.History;

[Mapper]
public static partial class BattleHistoryPlayerMapper
{
    public static partial BattleHistoryPlayer ToBattleHistoryPlayer(this BattleSelf battleSelf);
    public static partial BattleHistoryPlayer ToBattleHistoryPlayer(this BattleAlly battleAlly);
    public static partial BattleHistoryPlayer ToBattleHistoryPlayer(this BattleTarget battleTarget);
}