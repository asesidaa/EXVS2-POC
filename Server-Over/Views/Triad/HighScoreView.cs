using Microsoft.EntityFrameworkCore;

namespace ServerOver.Views.Triad;

[Keyless]
public class HighScoreView : TriadRankView
{
    public uint CourseHighScore { get; set; }
}