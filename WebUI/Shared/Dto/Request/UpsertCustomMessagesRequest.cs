using WebUI.Shared.Dto.Common;

namespace WebUI.Shared.Dto.Request;

public class UpsertCustomMessagesRequest : BasicCardRequest
{
    public CustomMessageGroupSetting MessageSetting { get; set; } = new();
}