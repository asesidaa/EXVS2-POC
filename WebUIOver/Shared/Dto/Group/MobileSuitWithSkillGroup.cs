using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Response;

namespace WebUIOver.Shared.Dto.Group;

public class MobileSuitWithSkillGroup
{
    public MobileSuit MobileSuit { get; set; }
    public MsSkillGroup? SkillGroup { get; set; }
}