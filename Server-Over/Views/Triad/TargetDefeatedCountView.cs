using Microsoft.EntityFrameworkCore;

namespace ServerOver.Views.Triad;

[Keyless]
public class TargetDefeatedCountView : TriadRankView
{
    public uint DestroyCount { get; set; }
}