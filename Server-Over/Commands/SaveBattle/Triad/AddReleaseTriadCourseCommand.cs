﻿using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Triad;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.Triad;

public class AddReleaseTriadCourseCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;

    public AddReleaseTriadCourseCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var triadInfoDomain = battleResultContext.TriadInfoDomain;
        var releaseCourseIds = triadInfoDomain.ReleasedCourseIds;

        if (releaseCourseIds is null)
        {
            return;
        }
        
        releaseCourseIds.ToList()
            .ForEach(releaseCourseId =>
            {
                var existingCourse = _context.TriadCourseDataDbSet
                    .FirstOrDefault(x => x.CardProfile == cardProfile && x.CourseId == releaseCourseId);

                if (existingCourse is not null)
                {
                    return;
                }

                _context.Add(new TriadCourseData()
                {
                    CardProfile = cardProfile,
                    CourseId = releaseCourseId,
                    ReleasedAt = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds(),
                    Highscore = 0,
                    TotalPlayNum = 0,
                    TotalClearNum = 0
                });

                _context.SaveChanges();
            });
    }
}