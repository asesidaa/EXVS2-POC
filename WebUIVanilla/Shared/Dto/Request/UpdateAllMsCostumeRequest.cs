using WebUIVanilla.Shared.Dto.Response;

namespace WebUIVanilla.Shared.Dto.Request;

public class UpdateAllMsCostumeRequest : BasicCardRequest
{
    public List<MsSkillGroup> MsSkillGroup { get; set; } = new();
}