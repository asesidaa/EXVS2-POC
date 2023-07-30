namespace WebUI.Shared.Dto.Common;

public class EchelonProfile
{
    public uint EchelonId { get; set; } = 0;
    // Will display the following
    // 上級大尉 (When EchelonId = 23 and SpecialEchelonFlag = true)
    // 准将 (When EchelonId = 38 and SpecialEchelonFlag = true)
    public bool SpecialEchelonFlag { get; set; } = false;
    public int EchelonExp { get; set; } = 0;
    // Show this flag only when [EchelonId = 23 or 38] and SpecialEchelonFlag = false
    // Can prepare Apply Button for [EchelonId = 23 or 38] and SpecialEchelonFlag = false
    public bool AppliedForSpecialEchelonTest { get; set; } = false; 
    public uint SpecialEchelonTestProgress { get; set; } = 0;
    public uint TotalWin { get; set; }
    public uint TotalLose { get; set; }
    public uint TotalRounds { get; set; }
    public uint ShuffleWin { get; set; }
    public uint ShuffleLose { get; set; }
    public uint ShuffleRounds { get; set; }
    public uint TeamWin { get; set; }
    public uint TeamLose { get; set; }
    public uint TeamRounds { get; set; }
}