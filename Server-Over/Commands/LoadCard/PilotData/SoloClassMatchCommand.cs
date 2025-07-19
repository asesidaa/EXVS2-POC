using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.PilotData;

public class SoloClassMatchCommand : IPilotDataCommand
{
    private readonly ServerDbContext _context;

    public SoloClassMatchCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.PilotDataGroup pilotDataGroup)
    {
        var soloClassMatchData = _context.SoloClassMatchRecordDbSet
            .First(x => x.CardProfile == cardProfile);

        pilotDataGroup.pilot_class_match.PilotClassMatchSolo =
            new Response.LoadCard.PilotDataGroup.PilotClassMatch.PilotClassMatchInfo()
            {
                ClassId = soloClassMatchData.ClassId,
                // TODO: Real time calculate rate
                Rate = soloClassMatchData.Rate,
                MaxPosition = soloClassMatchData.MaxPosition
            };
    }
}