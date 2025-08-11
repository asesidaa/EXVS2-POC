using nue.protocol.exvs;
using ServerOver.Constants;
using ServerOver.Context.Tracker;
using ServerOver.Models.Cards.Battle;
using ServerOver.Models.Cards.MobileSuit;
using ServerOver.Processor.Tracker.Rarity;
using WebUIOver.Shared.Dto.Enum;

namespace ServerOver.Processor.Tracker;

public class PilotTrackerProcessor : IStickerTrackerProcessor
{
    private readonly PilotTrackerContext _pilotTrackerContext;
    // Refer to https://w.atwiki.jp/exvs2ob/pages/42.html for how to calculate rarity
    private readonly IRarityProcessor _rarityProcessor;
    
    public PilotTrackerProcessor(PilotTrackerContext pilotTrackerContext, IRarityProcessor rarityProcessor)
    {
        _pilotTrackerContext = pilotTrackerContext;
        _rarityProcessor = rarityProcessor;
    }

    public void Process(Response.LoadCard.MobileUserGroup.PlayerSticker.Tracker tracker, MobileSuitPvPStatistic? mobileSuitPvPStatistic, PlayerLevel playerLevel)
    {
        switch ((TrackerName) tracker.TextId)
        {
            case TrackerName.PilotTotalBattleCount:
                tracker.Type = TrackerTypes.PilotStatisticType;
                tracker.Rarity = _rarityProcessor.Calculate(playerLevel, TrackerLevelRequirements.PilotTotalBattleCount);
                tracker.IntgerTrackerValue = _pilotTrackerContext.TotalBattleCount;
                return;
            case TrackerName.PilotTotalWinCount:
                tracker.Type = TrackerTypes.PilotStatisticType;
                tracker.Rarity = _rarityProcessor.Calculate(playerLevel, TrackerLevelRequirements.PilotTotalWinCount);
                tracker.IntgerTrackerValue = _pilotTrackerContext.TotalWinCount;
                return;
            case TrackerName.PilotTotalGivenDamage:
                tracker.Type = TrackerTypes.PilotStatisticType;
                tracker.Rarity = _rarityProcessor.Calculate(playerLevel, TrackerLevelRequirements.PilotTotalGivenDamage);
                tracker.IntgerTrackerValue = _pilotTrackerContext.TotalGivenDamage;
                return;
            case TrackerName.PilotTotalEnemyDefeatedCount:
                tracker.Type = TrackerTypes.PilotStatisticType;
                tracker.Rarity = _rarityProcessor.Calculate(playerLevel, TrackerLevelRequirements.PilotTotalEnemyDefeated);
                tracker.IntgerTrackerValue = _pilotTrackerContext.TotalEnemyDefeatedCount;
                return;
            case TrackerName.PilotTotalClassMatchTenConsecutiveWinCount:
                tracker.Type = TrackerTypes.PilotStatisticType;
                tracker.Rarity = _rarityProcessor.Calculate(playerLevel, TrackerLevelRequirements.PilotTotalClassMatchTenConsecutiveWinCount);
                tracker.IntgerTrackerValue = _pilotTrackerContext.TotalClassMatchTenConsecutiveWinCount;
                return;
            case TrackerName.PilotTotalNoDamageBattleCount:
                tracker.Type = TrackerTypes.PilotStatisticType;
                tracker.Rarity = _rarityProcessor.Calculate(playerLevel, TrackerLevelRequirements.PilotTotalNoDamageBattleCount);
                tracker.IntgerTrackerValue = _pilotTrackerContext.TotalNoDamageBattleCount;
                return;
            case TrackerName.PilotTotalExBurstDamage:
                tracker.Type = TrackerTypes.PilotStatisticType;
                tracker.Rarity = _rarityProcessor.Calculate(playerLevel, TrackerLevelRequirements.PilotTotalExBurstDamage);
                tracker.IntgerTrackerValue = _pilotTrackerContext.TotalExBurstDamage;
                return;
            case TrackerName.PilotTotalWinRate:
                tracker.Type = TrackerTypes.PilotStatisticType;
                tracker.Rarity = _rarityProcessor.Calculate(playerLevel, TrackerLevelRequirements.PilotTotalWinRate);
                
                if (_pilotTrackerContext.TotalBattleCount == 0)
                {
                    return;
                }

                var totalBattleCount = Convert.ToSingle(_pilotTrackerContext.TotalBattleCount);
                var totalWinCount = Convert.ToSingle(_pilotTrackerContext.TotalWinCount);
                var totalWinRate = 100 * (totalWinCount / totalBattleCount);
                
                tracker.FloatTrackerValue = totalWinRate;
                
                return;
            case TrackerName.PilotPlayerLevel:
                tracker.Type = TrackerTypes.PlayerLevelStatisticType;
                tracker.IntgerTrackerValue = _pilotTrackerContext.PlayerLevel;
                return;
            case TrackerName.PilotSoloClassRate:
                tracker.Type = TrackerTypes.SoloClassStatisticType;
                tracker.FloatTrackerValue = _pilotTrackerContext.SoloClassRate;
                return;
            case TrackerName.PilotTeamClassRate:
                tracker.Type = TrackerTypes.TeamClassStatisticRate;
                tracker.FloatTrackerValue = _pilotTrackerContext.TeamClassRate;
                return;
            case TrackerName.XbBattleCount:
                tracker.Type = TrackerTypes.XbBattleType;
                tracker.IntgerTrackerValue = 0;
                return;
            case TrackerName.LicenseScore:
                tracker.Type = TrackerTypes.LicenseScore;
                return;
            default:
                return;
        }
    }
}