using WebUIVanilla.Shared.Dto.Common;

namespace WebUIVanilla.Shared.Dto.Request;

public class UpsertGamepadConfigRequest : BasicCardRequest
{
    public GamepadConfig GamepadConfig { get; set; } = new();
}