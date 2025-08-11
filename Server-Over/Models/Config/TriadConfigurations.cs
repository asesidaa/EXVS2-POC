using System;

namespace ServerOver.Models.Config;

public class TriadConfigurations
{
    public uint[] TargetMsList { get; set; } = Array.Empty<uint>();
    public uint TimeAttackCourse { get; set; } = 1u;
    public uint HighScoreCourse { get; set; } = 1u;
}