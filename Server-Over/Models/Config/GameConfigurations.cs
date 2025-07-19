namespace ServerOver.Models.Config;

public class GameConfigurations
{
    public uint TrainingMinutes { get; set; }
    public uint[]? NewMobileSuits { get; set; } = null;
    public uint[]? UpdatedMobileSuits { get; set; } = null;
    public FesConfigurations FesConfigurations { get; set; } = new();
    public TriadConfigurations TriadConfigurations { get; set; } = new();
}