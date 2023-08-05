using WebUI.Shared.Dto.Common;

namespace WebUI.Shared.Dto.Json;

public class Navigator
{
    public uint Id { get; set; }
    public string NameEN { get; set; } = string.Empty;
    public string NameJP { get; set; } = string.Empty;
    public string NameCN { get; set; } = string.Empty;
    public List<IdValuePair>? Costumes { get; set; } = new ();
}