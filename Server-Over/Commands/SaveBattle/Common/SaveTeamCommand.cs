using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.Common;

public class SaveTeamCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;

    public SaveTeamCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var teamDomain = battleResultContext.TeamDomain;

        var userLeadingTeam = _context.TagTeamDataDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile && x.Id == teamDomain.TeamId);

        if (userLeadingTeam is null)
        {
            return;
        }

        userLeadingTeam.SkillPoint += teamDomain.TeamExp;
    }
}