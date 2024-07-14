using ServerVanilla.Context.Battle;
using ServerVanilla.Models.Cards;
using ServerVanilla.Models.Cards.Profile;
using ServerVanilla.Persistence;

namespace ServerVanilla.Command.SaveBattle.Common;

public class SaveEchelonCommand : ISaveBattleDataCommand
{
    private const uint MaxEchelon = 55;
    private const uint SCaptainEchelon = 23;
    private const uint SBrigadierEchelon = 38;
    private const uint GeneralEchelon = 54;
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

        battleProfile.SEchelonFlag = battleResultContext.EchelonDomain.SEchelonFlag;
        
        if (battleProfile.EchelonId == MaxEchelon)
        {
            return;
        }

        var currentEchelonData = _context.EchelonSettings
            .First(x => x.EchelonId == battleProfile.EchelonId);

        if (battleProfile.SEchelonMissionFlag)
        {
            HandleSpecialEchelonMission(battleProfile, battleResultContext);
            return;
        }

        battleProfile.EchelonExp += echelonDomain.EchelonExp;

        if (battleProfile.EchelonExp >= currentEchelonData.ExpWidth)
        {
            UpgradeEchelon(battleProfile);
            return;
        }
        
        if (currentEchelonData.DowngradeThreshold == 0)
        {
            AdjustNegativeExp(battleProfile);
            return;
        }

        if (battleProfile.EchelonExp <= currentEchelonData.DowngradeThreshold)
        {
            return;
        }
        
        DowngradeEchelon(battleProfile);
    }
    
    private void HandleSpecialEchelonMission(BattleProfile battleProfile, BattleResultContext battleResultContext)
    {
        battleProfile.SEchelonProgress = battleResultContext.EchelonDomain.SEchelonProgress;

        if (battleResultContext.EchelonDomain.SEchelonProgress < 3)
        {
            return;
        }
            
        battleProfile.SEchelonMissionFlag = false;

        if (battleProfile.EchelonId == SCaptainEchelon)
        {
            battleProfile.SEchelonProgress = 0;
            battleProfile.SEchelonFlag = true;
            battleProfile.SCaptainFlag = true;
            return;
        }

        if (battleProfile.EchelonId == SBrigadierEchelon)
        {
            battleProfile.SEchelonProgress = 0;
            battleProfile.SEchelonFlag = true;
            battleProfile.SBrigadierFlag = true;
        }
    }
    
    private void UpgradeEchelon(BattleProfile battleProfile)
    {
        if (battleProfile.EchelonId == GeneralEchelon)
        {
            battleProfile.EchelonExp = 500;
            return;
        }
        
        battleProfile.EchelonId += 1;
            
        var upgradeEchelonData = _context.EchelonSettings
            .First(x => x.EchelonId == battleProfile.EchelonId);

        battleProfile.EchelonExp = upgradeEchelonData.UpDefaultExp;
    }
    
    private void AdjustNegativeExp(BattleProfile battleProfile)
    {
        if (battleProfile.EchelonExp < 0)
        {
            battleProfile.EchelonExp = 0;
        }
    }

    private void DowngradeEchelon(BattleProfile battleProfile)
    {
        battleProfile.EchelonId -= 1;
        
        var downgradeEchelonData = _context.EchelonSettings
            .First(x => x.EchelonId == battleProfile.EchelonId);

        battleProfile.EchelonExp = downgradeEchelonData.DownDefaultExp;
    }
}