using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.PvP;

public class RemovePreBattleHistoryCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;
    
    public RemovePreBattleHistoryCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var preBattleHistory = _context.PreBattleHistoryDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile);

        if (preBattleHistory is null)
        {
            return;
        }

        _context.PreBattleHistoryDbSet.Remove(preBattleHistory);
        _context.SaveChanges();
    }
}