using WebUIOver.Shared.Dto.Message;

namespace WebUIOver.Shared.Dto.Request;

public class UpsertCustomMessagesRequest : BasicCardRequest
{
    public CustomMessageGroupSetting MessageSetting { get; set; } = new();
}