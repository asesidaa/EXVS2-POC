using WebUIOver.Shared.Dto.History;
using WebUIOver.Shared.Dto.Triad;

namespace WebUIOver.Client.Context.ViewCard;

public class ViewCardContext
{
    public string AccessCode { get; set; } = string.Empty;
    public string ChipId { get; set; } = string.Empty;
    public TriadCourseOverallResult TriadCourseOverallResult { get; set; } = new();
    public List<BattleHistorySummary> BattleHistorySummaries { get; set; } = new();
}