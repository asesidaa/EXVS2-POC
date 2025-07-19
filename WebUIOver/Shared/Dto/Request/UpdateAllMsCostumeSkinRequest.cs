using WebUIOver.Shared.Dto.Response;

namespace WebUIOver.Shared.Dto.Request;

public class UpdateAllMsCostumeSkinRequest : BasicCardRequest
{
    public List<MsSkillGroup> MsSkillGroup { get; set; } = new();
}