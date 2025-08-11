using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;
using ServerOver.Utils;

namespace ServerOver.Commands.LoadCard.PilotData;

public class TriadMiscInfoCommand : IPilotDataCommand
{
    private readonly ServerDbContext _context;

    public TriadMiscInfoCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.PilotDataGroup pilotDataGroup)
    {
        var triadMiscInfo = _context.TriadMiscInfoDbSet
            .First(x => x.CardProfile == cardProfile);
        
        pilotDataGroup.CpuRibbons = ArrayUtil.FromString(triadMiscInfo.CpuRibbons);
        pilotDataGroup.TotalTriadScore = triadMiscInfo.TotalTriadScore;
        pilotDataGroup.TotalTriadWantedDefeatNum = triadMiscInfo.TotalTriadWantedDefeatNum;
        pilotDataGroup.TotalTriadScenePlayNum = triadMiscInfo.TotalTriadScenePlayNum;
    }
}