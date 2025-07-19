using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.Team;

namespace ServerOver.Mapper.Card.Team;

[Mapper]
public static partial class TeamMapper
{
    [MapProperty(nameof(TagTeamData.TeamName), nameof(WebUIOver.Shared.Dto.Common.Team.Name))]
    public static partial WebUIOver.Shared.Dto.Common.Team ToTeam(this TagTeamData tagTeamData);
    
    public static partial TagTeamGroup ToTagTeamGroup(this WebUIOver.Shared.Dto.Common.Team team);
}