using System.Text.Json.Serialization;
using WebUIVanilla.Shared.Dto.Enum;

namespace WebUIVanilla.Shared.Dto.Common;

public class CustomMessage
{
    [JsonIgnore]
    public Command Command { get; set; } = Command.Up;
    public string MessageText { get; set; } = String.Empty;
    public uint UniqueMessageId { get; set; } = 0;
}