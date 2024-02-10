using nue.protocol.exvs;
using ServerVanilla.Models.Cards.MobileSuit;
using WebUIVanilla.Shared.Dto.Response;

namespace ServerVanilla.Mapper.Card.MobileSuit;

using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class MobileSuitUsageMapper
{
    public static partial Response.LoadCard.PilotDataGroup.MSSkillGroup ToMSSkillGroup(this MobileSuitUsage navi);
    public static partial MsSkillGroup ToMSSkillGroupDto(this MobileSuitUsage navi);
}