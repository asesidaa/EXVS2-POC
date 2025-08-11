using System.Text.Json.Serialization;
using WebUIOver.Shared.Dto.Enum;

namespace WebUIOver.Shared.Dto.Message;

public class CustomMessage
{
    [JsonIgnore]
    public Command Command { get; set; } = Command.Up;
    public string MessageText { get; set; } = String.Empty;
    public uint UniqueMessageId { get; set; } = 0;
}