using Microsoft.EntityFrameworkCore;

namespace ServerOver.Views.Triad;

[Keyless]
public class ClearTimeView : TriadRankView
{
    public uint CourseClearTime { get; set; }
}