using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.PvP;

public class SaveChallengeMissionDataCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;
    
    public SaveChallengeMissionDataCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var currentTime = DateTime.Now;
        var currentYear = (uint) currentTime.Year;
        var currentMonth = (uint) currentTime.Month;
        var currentDay = (uint) currentTime.Day;
        
        var challengeMissionProfile = _context.ChallengeMissionProfileDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile 
                                 && x.EffectiveYear == currentYear
                                 && x.EffectiveMonth == currentMonth
                                 && x.EffectiveDay == currentDay);

        if (challengeMissionProfile == null)
        {
            return;
        }
        
        var battleStatisticDomain = battleResultContext.BattleStatisticDomain;

        challengeMissionProfile.TotalBattleCount += 1;

        if (battleResultContext.CommonDomain.IsWin)
        {
            challengeMissionProfile.TotalBattleWinCount += 1;
        }

        if (battleStatisticDomain.ConsecutiveWinCount >= challengeMissionProfile.MaxConsecutiveWinCount)
        {
            challengeMissionProfile.MaxConsecutiveWinCount = battleStatisticDomain.ConsecutiveWinCount;
        }

        challengeMissionProfile.TotalDefeatCount += battleStatisticDomain.TotalEnemyDefeatedCount;
        challengeMissionProfile.TotalDamageCount += battleStatisticDomain.TotalGivenDamage;
    }
}