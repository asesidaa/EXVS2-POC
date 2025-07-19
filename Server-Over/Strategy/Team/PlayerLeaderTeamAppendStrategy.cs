using nue.protocol.exvs;
using ServerOver.Constants;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Strategy.Team;

public class PlayerLeaderTeamAppendStrategy : ITagTeamAppendStrategy
{
    private readonly ServerDbContext _context;

    public PlayerLeaderTeamAppendStrategy(ServerDbContext context)
    {
        _context = context;
    }
    
    public void Append(CardProfile cardProfile, List<TagTeamGroup> tagTeams)
    {
        _context.TagTeamDataDbSet
            .Where(x => x.CardProfile == cardProfile && x.CardId == cardProfile.Id)
            .ToList()
            .ForEach(tagTeam =>
            {
                tagTeams.Add(new TagTeamGroup()
                {
                    Id = (uint)tagTeam.Id,
                    Name = tagTeam.TeamName,
                    PartnerId = tagTeam.TeammateCardId,
                    SkillPoint = tagTeam.SkillPoint,
                    SkillPointBoost = TeamConstants.TagSkillPointBoost,
                    BackgroundPartsId = tagTeam.BackgroundPartsId,
                    EffectId = tagTeam.EffectId,
                    EmblemId = tagTeam.EmblemId,
                    BgmId = tagTeam.BgmId,
                    NameColorId = tagTeam.NameColorId,
                    BoostRemains = TeamConstants.BoostRemains
                });
            });
    }
}