using WebUIVanilla.Shared.Dto.Common;

namespace WebUIVanilla.Shared.Dto.Request;

public class UpdateCustomizeCommentRequest : BasicCardRequest
{
    public CustomizeComment CustomizeComment { get; set; } = new();
}