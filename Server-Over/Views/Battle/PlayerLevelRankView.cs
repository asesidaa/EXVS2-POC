using Microsoft.EntityFrameworkCore;

namespace ServerOver.Views.Battle;

[Keyless]
public class PlayerLevelRankView
{
    public uint Rank { get; set; } = 0;
    public string UserName { get; set; } = string.Empty;
    public uint PrestigeId { get; set; } = 0;
    public uint PlayerLevelId { get; set; } = 0;
    public uint PlayerExp { get; set; } = 0;
    public uint TotalWin { get; set; } = 0;
    public uint TotalLose { get; set; } = 0;
}