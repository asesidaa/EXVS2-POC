using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Shared.Dto.Response;

public class TeamResponse
{
    public string DistinctTeamFormationToken { get; set; } = string.Empty;
    public List<Team> Teams { get; set; } = new();
}