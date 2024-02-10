using ServerVanilla.Context.Battle;
using ServerVanilla.Models.Cards;
using ServerVanilla.Persistence;

namespace ServerVanilla.Command.SaveBattle.Common;

public class SaveEchelonCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;

    public SaveEchelonCommand(ServerDbContext context)
    {
        _context = context;
    }
    
    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var echelonDomain = battleResultContext.EchelonDomain;

        if (echelonDomain.EchelonExp == 0)
        {
            return;
        }

        var battleProfile = _context.BattleProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        var currentEchelonData = _context.EchelonSettings
            .First(x => x.EchelonId == battleProfile.EchelonId);

        battleProfile.EchelonExp += echelonDomain.EchelonExp;

        if (battleProfile.EchelonExp >= currentEchelonData.ExpWidth)
        {
            battleProfile.EchelonId += 1;
            battleProfile.EchelonExp = 0;
            return;
        }

        if (battleProfile.EchelonExp <= currentEchelonData.DowngradeThreshold)
        {
            battleProfile.EchelonId -= 1;
            
            var downgradeEchelonData = _context.EchelonSettings
                .First(x => x.EchelonId == battleProfile.EchelonId);

            battleProfile.EchelonExp = downgradeEchelonData.DownDefaultExp;
        }
    }
}