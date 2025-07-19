using ServerOver.Models.Cards.Battle;

namespace ServerOver.Processor.Tracker.Rarity;

public interface IRarityProcessor
{
    uint Calculate(PlayerLevel playerLevel, uint levelRequirement);
}