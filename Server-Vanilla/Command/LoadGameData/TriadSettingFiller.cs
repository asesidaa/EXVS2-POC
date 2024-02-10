using nue.protocol.exvs;

namespace ServerVanilla.Command.LoadGameData;

public class TriadSettingFiller : ILoadGameDataFiller
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        for (uint courseId = 1; courseId < 13; courseId++)
        {
            loadGameData.ReleaseCpuCourses.Add(new Response.LoadGameData.ReleaseCpuCourse()
            {
                CourseId = courseId,
                OpenedAt = 1514739500
            });
        }

        for (uint courseId = 16; courseId < 28; courseId++)
        {
            loadGameData.ReleaseCpuCourses.Add(new Response.LoadGameData.ReleaseCpuCourse()
            {
                CourseId = courseId,
                OpenedAt = 1514739500
            });
        }
        
        for (uint courseId = 31; courseId < 41; courseId++)
        {
            loadGameData.ReleaseCpuCourses.Add(new Response.LoadGameData.ReleaseCpuCourse()
            {
                CourseId = courseId,
                OpenedAt = 1514739500
            });
        }
        
        for (uint courseId = 46; courseId < 54; courseId++)
        {
            loadGameData.ReleaseCpuCourses.Add(new Response.LoadGameData.ReleaseCpuCourse()
            {
                CourseId = courseId,
                OpenedAt = 1514739500
            });
        }
        
        for (uint courseId = 100; courseId < 103; courseId++)
        {
            loadGameData.ReleaseCpuCourses.Add(new Response.LoadGameData.ReleaseCpuCourse()
            {
                CourseId = courseId,
                OpenedAt = 1514739500
            });
        }

        loadGameData.ReleaseCpuScenes = new[] { 1u, 2u, 3u, 4u, 5u };

        loadGameData.weekly_rank_info = new Response.LoadGameData.WeeklyRankInfo()
        {
            WeeklyRankNo = 0,
            WeeklyRankCourseId = 1
        };
    }
}