using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Shared.Dto.Request;

public class UpdateBasicProfileRequest : BasicCardRequest
{
    public BasicProfile BasicProfile { get; set; } = new();
}