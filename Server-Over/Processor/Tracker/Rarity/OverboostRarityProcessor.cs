using ServerOver.Models.Cards.Battle;

namespace ServerOver.Processor.Tracker.Rarity;

public class OverboostRarityProcessor : IRarityProcessor
{
    private const uint GoldBaseLine = 2000; 
    private const uint SilverBaseLine = 1000; 
    private const uint BronzeBaseLine = 0;

    private const uint GoldRarity = 3;
    private const uint SliverRarity = 2;
    private const uint BronzeRarity = 1;
    private const uint NormalRarity = 0;
    
    public uint Calculate(PlayerLevel playerLevel, uint levelRequirement)
    {
        var playerLevelId = playerLevel.PlayerLevelId;
        var playerPrestige = playerLevel.PrestigeId;
        
        var weightedLevel = playerPrestige * 1000 + playerLevelId;

        if (weightedLevel >= GoldBaseLine + levelRequirement)
        {
            return GoldRarity;
        }

        if (weightedLevel >= SilverBaseLine + levelRequirement)
        {
            return SliverRarity;
        }
        
        if (weightedLevel >= BronzeBaseLine + levelRequirement)
        {
            return BronzeRarity;
        }

        return NormalRarity;
    }
}