using Microsoft.EntityFrameworkCore;

namespace ServerOver.Views.Usage;

[Keyless]
public class BurstUsageView
{
    public uint BurstType { get; set; } = 0;
    public uint AggregatedTotalBattle { get; set; } = 0;
    public uint AggregatedTotalWin { get; set; } = 0;
}