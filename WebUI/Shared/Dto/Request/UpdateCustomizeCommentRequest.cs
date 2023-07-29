using WebUI.Shared.Dto.Common;

namespace WebUI.Shared.Dto.Request;

public class UpdateCustomizeCommentRequest : BasicCardRequest
{
    public CustomizeComment CustomizeComment { get; set; } = new();
}