using ServerVanilla.Context.Battle;
using ServerVanilla.Models.Cards;
using ServerVanilla.Persistence;

namespace ServerVanilla.Command.SaveBattle.Common;

public class SaveGpCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;

    public SaveGpCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        cardProfile.Gp += battleResultContext.CommonDomain.GpIncrement;
    }
}