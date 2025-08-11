using nue.protocol.exvs;
using ServerOver.Constants;
using ServerOver.Models.Cards.Battle;
using ServerOver.Models.Cards.MobileSuit;
using WebUIOver.Shared.Dto.Enum;

namespace ServerOver.Processor.Tracker;

public class MobileSuitTrackerProcessor : IStickerTrackerProcessor
{
    private readonly IStickerTrackerProcessor _pilotStickerTrackerProcessor;

    public MobileSuitTrackerProcessor(IStickerTrackerProcessor pilotStickerTrackerProcessor)
    {
        _pilotStickerTrackerProcessor = pilotStickerTrackerProcessor;
    }

    public void Process(Response.LoadCard.MobileUserGroup.PlayerSticker.Tracker tracker, MobileSuitPvPStatistic? mobileSuitPvPStatistic, PlayerLevel playerLevel)
    {
        if (mobileSuitPvPStatistic is null)
        {
            return;
        }
        
        switch ((TrackerName)tracker.TextId)
        {
            case TrackerName.MsTotalBattleCount:
                tracker.Type = TrackerTypes.MobileSuitStatisticType;
                tracker.IntgerTrackerValue = mobileSuitPvPStatistic.TotalBattleCount;
                return;
            case TrackerName.MsTotalWinCount:
                tracker.Type = TrackerTypes.MobileSuitStatisticType;
                tracker.IntgerTrackerValue = mobileSuitPvPStatistic.TotalWinCount;
                return;
            case TrackerName.MsTotalGivenDamage:
                tracker.Type = TrackerTypes.MobileSuitStatisticType;
                tracker.IntgerTrackerValue = mobileSuitPvPStatistic.TotalGivenDamage;
                return;
            case TrackerName.MsTotalEnemyDefeatedCount:
                tracker.Type = TrackerTypes.MobileSuitStatisticType;
                tracker.IntgerTrackerValue = mobileSuitPvPStatistic.TotalEnemyDefeatedCount;
                return;
            case TrackerName.MsTotalClassMatchTenConsecutiveWinCount:
                tracker.Type = TrackerTypes.MobileSuitStatisticType;
                tracker.IntgerTrackerValue = mobileSuitPvPStatistic.TotalClassMatchTenConsecutiveWinCount;
                return;
            case TrackerName.MsTotalNoDamageBattleCount:
                tracker.Type = TrackerTypes.MobileSuitStatisticType;
                tracker.IntgerTrackerValue = mobileSuitPvPStatistic.TotalNoDamageBattleCount;
                return;
            case TrackerName.MsTotalExBurstDamage:
                tracker.Type = TrackerTypes.MobileSuitStatisticType;
                tracker.IntgerTrackerValue = mobileSuitPvPStatistic.TotalExBurstDamage;
                return;
            case TrackerName.MsTotalWinRate:
                tracker.Type = TrackerTypes.MobileSuitStatisticType;
                
                if (mobileSuitPvPStatistic.TotalBattleCount == 0)
                {
                    return;
                }

                var totalBattleCount = Convert.ToSingle(mobileSuitPvPStatistic.TotalBattleCount);
                var totalWinCount = Convert.ToSingle(mobileSuitPvPStatistic.TotalWinCount);
                var totalWinRate = 100 * (totalWinCount / totalBattleCount);
                
                tracker.FloatTrackerValue = totalWinRate;

                return;
            default:
                _pilotStickerTrackerProcessor.Process(tracker, null, playerLevel);
                return;
        }
    }
}