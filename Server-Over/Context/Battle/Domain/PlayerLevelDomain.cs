namespace ServerOver.Context.Battle.Domain;

public class PlayerLevelDomain
{
    public uint LevelIdBefore { get; set; } = 0;
    public uint LevelIdAfter { get; set; } = 0;
    public uint ExpIncrement { get; set; } = 0;
    public uint PrestigeId { get; set; } = 0;
}