namespace ServerVanilla.Context.Battle.Domain.Triad;

public class TriadInfoDomain
{
    public uint CourseId { get; set; } = 0;
    public uint SceneSeq { get; set; } = 0;
    public uint SceneId { get; set; } = 0;
    public uint SceneType { get; set; } = 0;
    public uint CourseCategory { get; set; } = 0;
    public uint TotalWantedDefeatNum { get; set; } = 0;
    public uint SceneScore { get; set; } = 0;
    public uint CourseScore { get; set; } = 0;
    public bool? CourseClearFlag { get; set; } = false;
    public uint[]? ReleasedRibbonIds { get; set; } = Array.Empty<uint>();
    public uint[]? ReleasedCourseIds { get; set; } = Array.Empty<uint>();
}