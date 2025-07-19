using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Shared.Dto.Request;

public class UpsertTeamsRequest : BasicCardRequest
{
    public List<Team> Teams { get; set; } = new();
}