using WebUIVanilla.Shared.Dto.Common;

namespace WebUIVanilla.Shared.Dto.Request;

public class UpdateBasicProfileRequest : BasicCardRequest
{
    public BasicProfile BasicProfile { get; set; } = new();
}