using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Response;

namespace Server.Mappers;

[Mapper]
public static partial class MsSkillGroupMapper
{
    public static partial MsSkillGroup ToMsSkillGroupMapper(this Response.LoadCard.PilotDataGroup.MSSkillGroup mobileUserGroup);

    public static partial Response.LoadCard.PilotDataGroup.MSSkillGroup ToMsSkillGroupMapper(this MsSkillGroup mobileUserGroup);
}