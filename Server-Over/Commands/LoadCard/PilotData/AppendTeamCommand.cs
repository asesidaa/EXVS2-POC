using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Strategy.Team;

namespace ServerOver.Commands.LoadCard.PilotData;

public class AppendTeamCommand : IPilotDataCommand
{
    private readonly ITagTeamAppendStrategy _tagTeamAppendStrategy;

    public AppendTeamCommand(ITagTeamAppendStrategy tagTeamAppendStrategy)
    {
        _tagTeamAppendStrategy = tagTeamAppendStrategy;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.PilotDataGroup pilotDataGroup)
    {
        _tagTeamAppendStrategy.Append(cardProfile, pilotDataGroup.TagTeams);
    }
}