using WebUI.Shared.Dto.Common;

namespace WebUI.Shared.Dto.Request;

public class UpsertGamepadConfigRequest : BasicCardRequest
{
    public GamepadConfig GamepadConfig { get; set; } = new();
}