namespace ServerVanilla.Context.Battle.Domain;

public class NaviDomain
{
    public uint GuestNavId { get; set; } = 0;
    public uint GuestNavFamiliarityIncrement { get; set; } = 0;
    public uint BattleNavId { get; set; } = 0;
    public uint BattleNavFamiliarityIncrement { get; set; } = 0;
}