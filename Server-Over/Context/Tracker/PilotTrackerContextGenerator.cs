using ServerOver.Mapper.Card.Battle;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Context.Tracker;

public class PilotTrackerContextGenerator
{
    private readonly ServerDbContext _context;

    public PilotTrackerContextGenerator(ServerDbContext context)
    {
        _context = context;
    }
    
    public PilotTrackerContext Generate(CardProfile cardProfile)
    {
        var playerBattleStatistic = _context.PlayerBattleStatisticDbSet
            .First(x => x.CardProfile == cardProfile);
        
        var winLossRecord = _context.WinLossRecordDbSet
            .First(x => x.CardProfile == cardProfile);
        
        var playerLevel = _context.PlayerLevelDbSet
            .First(x => x.CardProfile == cardProfile);
        
        var soloClassMatchRecord = _context.SoloClassMatchRecordDbSet
            .First(x => x.CardProfile == cardProfile);
        
        var teamClassMatchRecord = _context.TeamClassMatchRecordDbSet
            .First(x => x.CardProfile == cardProfile);
        
        var pilotTrackerContext = playerBattleStatistic.ToPilotTrackerContext();

        pilotTrackerContext.TotalBattleCount = winLossRecord.TotalWin + winLossRecord.TotalLose;
        pilotTrackerContext.TotalWinCount = winLossRecord.TotalWin;
        pilotTrackerContext.PlayerLevel = playerLevel.PlayerLevelId;
        pilotTrackerContext.SoloClassRate = soloClassMatchRecord.Rate;
        pilotTrackerContext.TeamClassRate = teamClassMatchRecord.Rate;

        return pilotTrackerContext;
    }
}