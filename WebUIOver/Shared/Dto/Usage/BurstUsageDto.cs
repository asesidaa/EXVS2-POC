namespace WebUIOver.Shared.Dto.Usage;

public class BurstUsageDto
{
    public uint BurstType { get; set; } = 0;
    public uint AggregatedTotalBattle { get; set; } = 0;
    public uint AggregatedTotalWin { get; set; } = 0;
}