using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;
using ServerOver.Utils;
using WebUIOver.Shared.Dto.Enum;

namespace ServerOver.Commands.LoadCard.PilotData;

public class TriadHighScoreRankRibbonCommand : IPilotDataCommand
{
    private readonly ServerDbContext _context;

    public TriadHighScoreRankRibbonCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.PilotDataGroup pilotDataGroup)
    {
        var rankRecord = _context.HighScoreViews
            .FirstOrDefault(x => x.CardId == cardProfile.Id && x.Rank <= TriadRankConstants.Top20);

        if (rankRecord is null)
        {
            return;
        }

        if (rankRecord.Rank == 1)
        {
            pilotDataGroup.CpuRibbons = pilotDataGroup.CpuRibbons.Concat(new [] { TriadRankConstants.HighScoreGold }).ToArray();
            return;
        }
        
        if (rankRecord.Rank == 2)
        {
            pilotDataGroup.CpuRibbons = pilotDataGroup.CpuRibbons.Concat(new [] { TriadRankConstants.HighScoreSilver }).ToArray();
            return;
        }
        
        if (rankRecord.Rank == 3)
        {
            pilotDataGroup.CpuRibbons = pilotDataGroup.CpuRibbons.Concat(new [] { TriadRankConstants.HighScoreBronze }).ToArray();
            return;
        }
        
        pilotDataGroup.CpuRibbons = pilotDataGroup.CpuRibbons.Concat(new [] { TriadRankConstants.HighScoreMetalic }).ToArray();
    }
}