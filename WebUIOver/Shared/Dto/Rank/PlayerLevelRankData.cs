using WebUIOver.Shared.Dto.Player;

namespace WebUIOver.Shared.Dto.Rank;

public class PlayerLevelRankData
{
    public List<PlayerLevelRankDto> PlayerLevelRanks { get; set; } = new();
    public ExpRequirement ExpRequirement { get; set; } = new();
}