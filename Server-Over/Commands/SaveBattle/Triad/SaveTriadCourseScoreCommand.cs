using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Triad;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.Triad;

public class SaveTriadCourseScoreCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;

    public SaveTriadCourseScoreCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var isWin = battleResultContext.CommonDomain.IsWin;
        var triadInfoDomain = battleResultContext.TriadInfoDomain;

        var triadCourseData = _context.TriadCourseDataDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile && x.CourseId == triadInfoDomain.CourseId);

        var courseClearFlag = triadInfoDomain.CourseClearFlag.GetValueOrDefault(false);
        
        if (triadCourseData is null)
        {
            var newTriadCourseData = new TriadCourseData()
            {
                CardProfile = cardProfile,
                CourseId = triadInfoDomain.CourseId,
                ReleasedAt = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds(),
                Highscore = triadInfoDomain.SceneScore,
                TotalPlayNum = courseClearFlag ? 1u : 0u,
            };

            if (isWin)
            {
                newTriadCourseData.TotalClearNum = courseClearFlag ? 1u : 0u;
            }

            _context.Add(newTriadCourseData);
            _context.SaveChanges();

            return;
        }

        if (triadInfoDomain.CourseScore >= triadCourseData.Highscore)
        {
            triadCourseData.Highscore = triadInfoDomain.CourseScore;
        }

        triadCourseData.TotalPlayNum += courseClearFlag ? 1u : 0u;

        if (isWin)
        {
            triadCourseData.TotalClearNum += courseClearFlag ? 1u : 0u;
        }
    }
}