using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Triad.Ranking;
using ServerOver.Models.Config;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.Triad.Ranking;

public class SaveTriadRankHighScoreCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;
    private readonly CardServerConfig _config;
    private readonly uint _year;
    private readonly uint _month;
    
    public SaveTriadRankHighScoreCommand(ServerDbContext context, CardServerConfig config, uint year, uint month)
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

        if (triadInfoDomain.CourseId != _config.GameConfigurations.TriadConfigurations.HighScoreCourse)
        {
            return;
        }
        
        if (triadInfoDomain.CourseScore == 0)
        {
            return;
        }

        var highScoreRecord = _context.TriadHighScoreDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile && x.Year == _year && x.Month == _month);

        if (highScoreRecord is null)
        {
            var triadHighScore = new TriadHighScore()
            {
                CardProfile = cardProfile,
                Year = _year,
                Month = _month,
                CourseHighScore = triadInfoDomain.CourseScore
            };
            
            _context.Add(triadHighScore);
            _context.SaveChanges();
            return;
        }

        if (triadInfoDomain.CourseScore <= highScoreRecord.CourseHighScore)
        {
            return;
        }
        
        highScoreRecord.CourseHighScore = triadInfoDomain.CourseScore;
        _context.SaveChanges();
    }
}