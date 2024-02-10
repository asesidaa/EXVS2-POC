using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerVanilla.Models.Cards.Message;

[Table("exvs2_playing_message")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class PlayingMessage : Message
{
}