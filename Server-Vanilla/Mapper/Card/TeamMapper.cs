using Riok.Mapperly.Abstractions;
using ServerVanilla.Models.Cards.Team;
using WebUIVanilla.Shared.Dto.Common;

namespace ServerVanilla.Mapper.Card;

[Mapper]
public static partial class TeamMapper
{
    [MapProperty(nameof(TagTeamData.TeamName), nameof(Team.Name))]
    public static partial Team ToTeam(this TagTeamData tagTeamData);
}