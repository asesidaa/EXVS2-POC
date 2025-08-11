namespace WebUIOver.Shared.Dto.Common;

public class Familiarity
{
    public uint Id { get; set; } = 0;
    public bool UseDescription { get; set; }= false;
    public string Description { get; set; } = string.Empty;
    public uint Level { get; set; }= 0;
    public string ColorCode { get; set; } = string.Empty;
    public uint MinimumPoint { get; set; } = 0;
}