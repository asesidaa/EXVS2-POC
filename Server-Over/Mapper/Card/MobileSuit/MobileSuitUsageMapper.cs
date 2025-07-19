using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.MobileSuit;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Mapper.Card.MobileSuit;

[Mapper]
public static partial class MobileSuitUsageMapper
{
    public static partial Response.LoadCard.PilotDataGroup.MSSkillGroup ToMSSkillGroup(this MobileSuitUsage navi);
    public static partial MsSkillGroup ToMSSkillGroupDto(this MobileSuitUsage navi);
}