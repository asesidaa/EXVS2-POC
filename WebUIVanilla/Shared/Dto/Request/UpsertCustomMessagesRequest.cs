using WebUIVanilla.Shared.Dto.Common;

namespace WebUIVanilla.Shared.Dto.Request;

public class UpsertCustomMessagesRequest : BasicCardRequest
{
    public CustomMessageGroupSetting MessageSetting { get; set; } = new();
}