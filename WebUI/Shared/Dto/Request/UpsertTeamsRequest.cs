using WebUI.Shared.Dto.Common;

namespace WebUI.Shared.Dto.Request;

public class UpsertTeamsRequest : BasicCardRequest
{
    public List<Team> Teams { get; set; } = new();
}