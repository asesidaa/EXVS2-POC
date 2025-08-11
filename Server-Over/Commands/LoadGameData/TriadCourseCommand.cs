using nue.protocol.exvs;

namespace ServerOver.Commands.LoadGameData;

public class TriadCourseCommand : ILoadGameDataCommand
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        // 200 - 212 are F Course 1-13
        loadGameData.ReleaseCpuCourses.AddRange(Enumerable.Range(1, 212).Select(i =>
            new Response.LoadGameData.ReleaseCpuCourse
            {
                CourseId = (uint)i,
                OpenedAt = (ulong)(DateTimeOffset.Now - TimeSpan.FromDays(10)).ToUnixTimeSeconds()
            }));
        
        loadGameData.ReleaseCpuCourses.AddRange(Enumerable.Range(250, 266).Select(i =>
            new Response.LoadGameData.ReleaseCpuCourse
            {
                CourseId = (uint)i,
                OpenedAt = (ulong)(DateTimeOffset.Now - TimeSpan.FromDays(10)).ToUnixTimeSeconds()
            }));
    }
}