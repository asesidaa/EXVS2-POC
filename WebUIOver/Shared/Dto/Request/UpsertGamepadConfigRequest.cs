using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Shared.Dto.Request;

public class UpsertGamepadConfigRequest : BasicCardRequest
{
    public GamepadConfig GamepadConfig { get; set; } = new();
}