using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.MobileSuit;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Enum;

namespace ServerOver.Commands.SaveBattle.PvP;

public class SaveMobileSuitTrackerStatCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;

    public SaveMobileSuitTrackerStatCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var commonDomain = battleResultContext.CommonDomain;
        var mobileSuitMasteryDomain = battleResultContext.MobileSuitMasteryDomain;
        var battleStatisticDomain = battleResultContext.BattleStatisticDomain;
        var isWin = commonDomain.IsWin;
        var noDamageBattleCount = battleStatisticDomain.NoDamageFlag ? 1u : 0u;
        var tenConsecutiveWinCount = battleStatisticDomain.ConsecutiveWinCount == 10 ? 1u : 0u;
        
        var mobileSuitPvPStatistic = _context.MobileSuitPvPStatisticDbSet
            .FirstOrDefault(x => 
                x.CardProfile == cardProfile && 
                x.MstMobileSuitId == mobileSuitMasteryDomain.ActualMobileSuitId
            );

        if (mobileSuitPvPStatistic is null)
        {
            var newMsPvPStatistic = new MobileSuitPvPStatistic()
            {
                CardProfile = cardProfile,
                MstMobileSuitId = mobileSuitMasteryDomain.ActualMobileSuitId,
                TotalBattleCount = 1,
                TotalWinCount = isWin ? 1u : 0u,
                TotalGivenDamage = battleStatisticDomain.TotalGivenDamage,
                TotalEnemyDefeatedCount = battleStatisticDomain.TotalEnemyDefeatedCount,
                TotalNoDamageBattleCount = noDamageBattleCount,
                TotalExBurstDamage = battleStatisticDomain.TotalExBurstDamage
            };

            if (commonDomain.BattleMode == BattleModeConstant.ClassMatchSolo 
                || commonDomain.BattleMode == BattleModeConstant.ClassMatchTeam)
            {
                newMsPvPStatistic.TotalClassMatchTenConsecutiveWinCount = tenConsecutiveWinCount;
            }
            
            _context.Add(newMsPvPStatistic);
            _context.SaveChanges();

            return;
        }

        mobileSuitPvPStatistic.TotalBattleCount += 1;
        mobileSuitPvPStatistic.TotalWinCount += isWin ? 1u : 0u;
        mobileSuitPvPStatistic.TotalGivenDamage += battleStatisticDomain.TotalGivenDamage;
        mobileSuitPvPStatistic.TotalEnemyDefeatedCount += battleStatisticDomain.TotalEnemyDefeatedCount;
        mobileSuitPvPStatistic.TotalNoDamageBattleCount += battleStatisticDomain.NoDamageFlag ? 1u : 0u;
        mobileSuitPvPStatistic.TotalExBurstDamage += battleStatisticDomain.TotalExBurstDamage;
        
        if (commonDomain.BattleMode == BattleModeConstant.ClassMatchSolo 
            || commonDomain.BattleMode == BattleModeConstant.ClassMatchTeam)
        {
            mobileSuitPvPStatistic.TotalClassMatchTenConsecutiveWinCount += tenConsecutiveWinCount;
        }
    }
}