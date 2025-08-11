namespace WebUIOver.Shared.Dto.Common;

public class Navigator : IdValuePair
{
    public List<IdValuePair>? Costumes { get; set; } = new();
    public string Series { get; set; } = string.Empty;
    public string SeriesJP { get; set; } = string.Empty;
    public string SeriesCN { get; set; } = string.Empty;
    public string SeriesTC { get; set; } = string.Empty;
    public string SeriesTC2 { get; set; } = string.Empty;
    public string Seiyuu { get; set; } = string.Empty;
    public string SeiyuuJP { get; set; } = string.Empty;
    public string SeiyuuCN { get; set; } = string.Empty;
    public string SeiyuuTC { get; set; } = string.Empty;
    public string SeiyuuTC2 { get; set; } = string.Empty;
    public int ClosenessPoint { get; set; } = 0;
    public Familiarity FamiliarityDomain { get; set; } = new();
}