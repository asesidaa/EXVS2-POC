namespace WebUIOver.Shared.Dto.Triad;

public class TriadCourseOverallResult
{
    public List<TriadCourseResult> TriadCourseResults { get; set; } = new();
    public uint[] CpuRibbons { get; set; } = Array.Empty<uint>();
}