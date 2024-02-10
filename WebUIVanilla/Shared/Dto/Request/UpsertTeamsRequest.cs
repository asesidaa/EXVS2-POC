using WebUIVanilla.Shared.Dto.Common;

namespace WebUIVanilla.Shared.Dto.Request;

public class UpsertTeamsRequest : BasicCardRequest
{
    public List<Team> Teams { get; set; } = new();
}