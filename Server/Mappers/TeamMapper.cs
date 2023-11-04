using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using Server.Models.Cards;
using WebUI.Shared.Dto.Common;

namespace Server.Mappers;

[Mapper]
public static partial class TeamMapper
{
    [MapProperty(nameof(TagTeamData.TeamName), nameof(Team.Name))]
    public static partial Team ToTeam(this TagTeamData tagTeamData);
    public static partial TagTeamGroup ToTagTeamGroup(this Team team);
}