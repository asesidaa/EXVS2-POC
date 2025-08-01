﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Message;

[Table("exvs2ob_playing_message")]
[Index(nameof(Id))]
[Index(nameof(MessageSettingId))]
public class PlayingMessage : Message
{
}