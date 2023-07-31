using System.Text.Json.Serialization;
using WebUI.Shared.Dto.Enum;

namespace WebUI.Shared.Dto.Common;

public class CustomMessage
{
    [JsonIgnore]
    public Command Command { get; set; } = Command.Up;
    public string MessageText { get; set; } = String.Empty;
    public uint UniqueMessageId { get; set; } = 0;
}