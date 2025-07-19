using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Message;

[Table("exvs2ob_online_shuffle_playing_message")]
[Index(nameof(Id))]
[Index(nameof(MessageSettingId))]
public class OnlineShufflePlayingMessage : Message
{
}