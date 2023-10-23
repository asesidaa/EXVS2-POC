using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using WebUI.Shared.Dto.Common;

namespace Server.Mappers;

[Mapper]
public static partial class TeamMapper
{
    public static partial Team ToTeam(this TagTeamGroup tagTeamGroup);
    public static partial TagTeamGroup ToTagTeamGroup(this Team team);
}