using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;
using ServerOver.Utils;
using WebUIOver.Shared.Dto.Enum;

namespace ServerOver.Commands.LoadCard.PilotData;

public class TriadTimeAttackRankRibbonCommand : IPilotDataCommand
{
    private readonly ServerDbContext _context;

    public TriadTimeAttackRankRibbonCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.PilotDataGroup pilotDataGroup)
    {
        var rankRecord = _context.ClearTimeViews
            .FirstOrDefault(x => x.CardId == cardProfile.Id && x.Rank <= TriadRankConstants.Top20);

        if (rankRecord is null)
        {
            return;
        }

        if (rankRecord.Rank == 1)
        {
            pilotDataGroup.CpuRibbons = pilotDataGroup.CpuRibbons.Concat(new [] { TriadRankConstants.TimeAttackGold }).ToArray();
            return;
        }
        
        if (rankRecord.Rank == 2)
        {
            pilotDataGroup.CpuRibbons = pilotDataGroup.CpuRibbons.Concat(new [] { TriadRankConstants.TimeAttackSilver }).ToArray();
            return;
        }
        
        if (rankRecord.Rank == 3)
        {
            pilotDataGroup.CpuRibbons = pilotDataGroup.CpuRibbons.Concat(new [] { TriadRankConstants.TimeAttackBronze }).ToArray();
            return;
        }
        
        pilotDataGroup.CpuRibbons = pilotDataGroup.CpuRibbons.Concat(new [] { TriadRankConstants.TimeAttackMetalic }).ToArray();
    }
}