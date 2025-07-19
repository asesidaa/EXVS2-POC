using nue.protocol.exvs;
using ServerOver.Mapper.Card.MobileSuit;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.PilotData;

public class PilotMobileSuitUsageCommand : IPilotDataCommand
{
    private readonly ServerDbContext _context;

    public PilotMobileSuitUsageCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.PilotDataGroup pilotDataGroup)
    {
        _context.MobileSuitUsageDbSet
            .Where(x => x.CardProfile == cardProfile)
            .ToList()
            .ForEach(mobileSuitUsage => pilotDataGroup.MsSkills.Add(mobileSuitUsage.ToMSSkillGroup()));
    }
}