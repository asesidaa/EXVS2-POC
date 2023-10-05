namespace WebUI.Shared.Dto.Common;

public class MobileSuit : IdValuePair
{
    public string Series { get; set; } = string.Empty;
    public uint Cost { get; set; } = 0;
    public List<IdValuePair>? Costumes { get; set; } = new();
    public int MasteryPoint { get; set; } = 0;
    public Familiarity MasteryDomain { get; set; } = new();
}