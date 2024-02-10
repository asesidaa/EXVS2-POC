using ServerVanilla.Context.Battle;
using ServerVanilla.Models.Cards;
using ServerVanilla.Persistence;

namespace ServerVanilla.Command.SaveBattle.Common;

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

        var userLeadingTeam = _context.TagTeamData
            .FirstOrDefault(x => x.CardProfile == cardProfile && x.Id == teamDomain.TeamId);

        if (userLeadingTeam is null)
        {
            return;
        }

        userLeadingTeam.SkillPoint += teamDomain.TeamExp;
    }
}