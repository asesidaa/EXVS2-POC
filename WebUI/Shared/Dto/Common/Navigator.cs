namespace WebUI.Shared.Dto.Common;

public class Navigator : IdValuePair
{
    public List<IdValuePair>? Costumes { get; set; } = new();
    public string? Series { get; set; } = string.Empty;
    public string? Seiyuu { get; set; } = string.Empty;
    public int ClosenessPoint { get; set; } = 0;
    public Familiarity FamiliarityDomain { get; set; } = new();
}