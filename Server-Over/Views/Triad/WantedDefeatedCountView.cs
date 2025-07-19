using Microsoft.EntityFrameworkCore;

namespace ServerOver.Views.Triad;

[Keyless]
public class WantedDefeatedCountView : TriadRankView
{
    public uint DestroyCount { get; set; }
}