namespace WebUI.Shared.Dto.Common;

public class TagTeamMastery
{
    public uint Id { get; set; } = 0;
    public string Level { get; set; } = string.Empty;
    public uint MinimumPoint { get; set; } = 0;
    public uint? ToNextLevel { get; set; }
    public string BackgroundColor { get; set; } = string.Empty;
}