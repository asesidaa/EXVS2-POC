using nue.protocol.exvs;
using ServerOver.Models.Cards;

namespace ServerOver.Strategy.Team;

public interface ITagTeamAppendStrategy
{
    void Append(CardProfile cardProfile, List<TagTeamGroup> tagTeams);
}