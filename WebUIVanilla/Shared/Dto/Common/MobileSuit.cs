namespace WebUIVanilla.Shared.Dto.Common;

public class MobileSuit : IdValuePair
{
    public string Pilot { get; set; } = string.Empty;
    public string PilotJP { get; set; } = string.Empty;
    public string PilotCN { get; set; } = string.Empty;
    public string PilotSeiyuu { get; set; } = string.Empty;
    public string PilotSeiyuuJP { get; set; } = string.Empty;
    public string PilotSeiyuuCN { get; set; } = string.Empty;
    public string Series { get; set; } = string.Empty;
    public string SeriesJP { get; set; } = string.Empty;
    public string SeriesCN { get; set; } = string.Empty;
    public uint Cost { get; set; } = 0;
    public List<IdValuePair>? Costumes { get; set; } = new();
    public int MasteryPoint { get; set; } = 0;
    public Familiarity MasteryDomain { get; set; } = new();
    public bool IsFavouriteMs { get; set; } = true;
    public double TotalBattleCount { get; set; } = 0;
    public double WinCount { get; set; } = 0;
    public double WinRate { get; set; } = 0;
    public double TotalAgainstBattleCount { get; set; } = 0;
    public double WinAgainstCount { get; set; } = 0;
    public double WinAgainstRate { get; set; } = 0;
}