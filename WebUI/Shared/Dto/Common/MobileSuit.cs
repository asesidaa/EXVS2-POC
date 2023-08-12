namespace WebUI.Shared.Dto.Common;

public class MobileSuit : IdValuePair
{
    public List<IdValuePair>? Costumes { get; set; } = new();
}