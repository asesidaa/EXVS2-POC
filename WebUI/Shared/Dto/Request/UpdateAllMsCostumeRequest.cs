using WebUI.Shared.Dto.Response;

namespace WebUI.Shared.Dto.Request;

public class UpdateAllMsCostumeRequest : BasicCardRequest
{
    public List<MsSkillGroup> MsSkillGroup { get; set; } = new();
}