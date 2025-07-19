namespace WebUIOver.Shared.Dto.Common;

public class MobileSuit : IdValuePair, ICloneable
{
    public string Pilot { get; set; } = string.Empty;
    public string PilotJP { get; set; } = string.Empty;
    public string PilotCN { get; set; } = string.Empty;
    public string PilotTC { get; set; } = string.Empty;
    public string PilotTC2 { get; set; } = string.Empty;
    public string PilotSeiyuu { get; set; } = string.Empty;
    public string PilotSeiyuuJP { get; set; } = string.Empty;
    public string PilotSeiyuuCN { get; set; } = string.Empty;
    public string PilotSeiyuuTC { get; set; } = string.Empty;
    public string PilotSeiyuuTC2 { get; set; } = string.Empty;
    public string Series { get; set; } = string.Empty;
    public string SeriesJP { get; set; } = string.Empty;
    public string SeriesCN { get; set; } = string.Empty;
    public string SeriesTC { get; set; } = string.Empty;
    public string SeriesTC2 { get; set; } = string.Empty;
    public uint Cost { get; set; } = 0;
    public List<IdValuePair>? Costumes { get; set; } = new();
    public List<IdValuePair>? Skins { get; set; } = new();
    public List<IdValuePair>? Poses { get; set; } = new();
    public int MasteryPoint { get; set; } = 0;
    public Familiarity MasteryDomain { get; set; } = new();
    public bool IsFavouriteMs { get; set; } = true;
    public double TotalBattleCount { get; set; } = 0;
    public double WinCount { get; set; } = 0;
    public double WinRate { get; set; } = 0;
    public double TotalAgainstBattleCount { get; set; } = 0;
    public double WinAgainstCount { get; set; } = 0;
    public double WinAgainstRate { get; set; } = 0;

    public object Clone()
    {
        var clonedMs = new MobileSuit()
        {
            Id = Id,
            Value = Value,
            ValueJP = ValueJP,
            ValueCN = ValueCN,
            ValueTC = ValueTC,
            ValueTC2 = ValueTC2,
            Pilot = Pilot,
            PilotJP = PilotJP,
            PilotCN = PilotCN,
            PilotTC = PilotTC,
            PilotTC2 = PilotTC2,
            PilotSeiyuu = PilotSeiyuu,
            PilotSeiyuuJP = PilotSeiyuuJP,
            PilotSeiyuuCN = PilotSeiyuuCN,
            PilotSeiyuuTC = PilotSeiyuuTC,
            PilotSeiyuuTC2 = PilotSeiyuuTC2,
            Series = Series,
            SeriesJP = SeriesJP,
            SeriesCN = SeriesCN,
            SeriesTC = SeriesTC,
            SeriesTC2 = SeriesTC2,
            Cost = Cost,
            Costumes = Costumes,
            Skins = Skins,
            Poses = Poses,
            MasteryPoint = MasteryPoint,
            IsFavouriteMs = IsFavouriteMs,
            TotalBattleCount = TotalBattleCount,
            WinCount = WinCount,
            WinRate = WinRate,
            TotalAgainstBattleCount = TotalAgainstBattleCount,
            WinAgainstCount = WinAgainstCount,
            WinAgainstRate = WinAgainstRate
        };

        return clonedMs;
    }
}