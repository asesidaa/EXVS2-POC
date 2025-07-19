using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Triad.Ranking;
using ServerOver.Models.Config;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.Triad.Ranking;

public class SaveTriadRankClearTimeCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;
    private readonly CardServerConfig _config;
    private readonly uint _year;
    private readonly uint _month;
    
    public SaveTriadRankClearTimeCommand(ServerDbContext context, CardServerConfig config, uint year, uint month)
    {
        _context = context;
        _config = config;
        _year = year;
        _month = month;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var isWin = battleResultContext.CommonDomain.IsWin;

        if (!isWin)
        {
            return;
        }
        
        var triadInfoDomain = battleResultContext.TriadInfoDomain;
        var courseClearFlag = triadInfoDomain.CourseClearFlag.GetValueOrDefault(false);

        if (!courseClearFlag)
        {
            return;
        }

        if (triadInfoDomain.CourseId != _config.GameConfigurations.TriadConfigurations.TimeAttackCourse)
        {
            return;
        }
        
        if (triadInfoDomain.CourseClearTime == 0)
        {
            return;
        }

        var timeAttackRecord = _context.TriadClearTimeDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile && x.Year == _year && x.Month == _month);

        if (timeAttackRecord is null)
        {
            var triadClearTime = new TriadClearTime()
            {
                CardProfile = cardProfile,
                Year = _year,
                Month = _month,
                CourseClearTime = triadInfoDomain.CourseClearTime
            };
            
            _context.Add(triadClearTime);
            _context.SaveChanges();
            return;
        }

        if (triadInfoDomain.CourseClearTime >= timeAttackRecord.CourseClearTime)
        {
            return;
        }
        
        timeAttackRecord.CourseClearTime = triadInfoDomain.CourseClearTime;
        _context.SaveChanges();
    }
}