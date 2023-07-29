using WebUI.Shared.Dto.Common;

namespace WebUI.Shared.Dto.Request;

public class UpdateBasicProfileRequest : BasicCardRequest
{
    public BasicProfile BasicProfile { get; set; } = new();
}