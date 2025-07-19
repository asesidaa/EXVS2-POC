using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Battle.History;

[Table("exvs2ob_battle_target")]
[Index(nameof(Id))]
[Index(nameof(Id), nameof(BattleHistoryId))]
[Index(nameof(BattleHistoryId), nameof(CardId))]
public class BattleTarget : BattlePerson
{
    
}