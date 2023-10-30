namespace WebUI.Shared.Dto.Common;

public class FullMsBattleRecord
{
    public uint MsId { get; set; } = 0;
    public uint OfflineShuffleWinCount { get; set; } = 0;
    public uint OfflineShuffleLossCount { get; set; } = 0;
    public uint OfflineTeamWinCount { get; set; } = 0;
    public uint OfflineTeamLossCount { get; set; } = 0;
}