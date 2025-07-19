using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.PilotData;

public class TeamClassMatchCommand : IPilotDataCommand
{
    private readonly ServerDbContext _context;

    public TeamClassMatchCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.PilotDataGroup pilotDataGroup)
    {
        var teamClassMatchData = _context.TeamClassMatchRecordDbSet
            .First(x => x.CardProfile == cardProfile);

        pilotDataGroup.pilot_class_match.PilotClassMatchTeam =
            new Response.LoadCard.PilotDataGroup.PilotClassMatch.PilotClassMatchInfo()
            {
                ClassId = teamClassMatchData.ClassId,
                // TODO: Real time calculate rate
                Rate = teamClassMatchData.Rate,
                MaxPosition = teamClassMatchData.MaxPosition
            };
    }
}