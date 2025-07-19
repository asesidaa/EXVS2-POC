using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Battle;

[Table("exvs2ob_battle_team_class_record")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class TeamClassMatchRecord : BaseClassMatchRecord
{
    
}