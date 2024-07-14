using nue.protocol.exvs;
using Server.Context.Battle;
using Server.Models.Cards;
using Server.Persistence;
using WebUI.Shared.Dto.Enum;

namespace Server.Command.SaveBattle;

public class SaveEchelonCommand : ISaveBattleCommand
{
    private const uint MaxEchelon = 55;
    private const uint MaxCpuEchelon = 19;
    private const uint SCaptainEchelon = 23;
    private const uint SBrigadierEchelon = 38;
    private const uint GeneralEchelon = 54;
    private readonly ServerDbContext _context;

    public SaveEchelonCommand(ServerDbContext context)
    {
        _context = context;
    }
    
    public void Save(CardProfile cardProfile, Response.PreLoadCard.LoadPlayer loadPlayer, Response.PreLoadCard.MobileUserGroup user, Response.LoadCard.PilotDataGroup pilotData,
        Response.LoadCard.MobileUserGroup mobileUser, BattleResultContext battleResultContext)
    {
        var echelonContext = battleResultContext.EchelonDomain;
        loadPlayer.SEchelonFlag = battleResultContext.EchelonDomain.SEchelonFlag;
        
        if (loadPlayer.EchelonId == MaxEchelon)
        {
            return;
        }

        if (battleResultContext.CommonDomain.BattleMode == BattleModeConstant.Triad &&
            loadPlayer.EchelonId >= MaxCpuEchelon)
        {
            return;
        }

        if (loadPlayer.SEchelonMissionFlag)
        {
            HandleSpecialEchelonMission(loadPlayer, battleResultContext);
            return;
        }
        
        var currentEchelonData = _context.EchelonSettings
            .First(x => x.EchelonId == loadPlayer.EchelonId);
        
        loadPlayer.EchelonExp += echelonContext.EchelonExp;

        if (loadPlayer.EchelonExp >= currentEchelonData.ExpWidth)
        {
            
            UpgradeEchelon(loadPlayer);
            return;
        }

        if (currentEchelonData.DowngradeThreshold == 0)
        {
            AdjustNegativeExp(loadPlayer);
            return;
        }

        if (loadPlayer.EchelonExp > currentEchelonData.DowngradeThreshold)
        {
            return;
        }

        DowngradeEchelon(loadPlayer);
    }
    
    private void HandleSpecialEchelonMission(Response.PreLoadCard.LoadPlayer loadPlayer, BattleResultContext battleResultContext)
    {
        loadPlayer.SEchelonProgress = battleResultContext.EchelonDomain.SEchelonProgress;

        if (battleResultContext.EchelonDomain.SEchelonProgress < 3)
        {
            return;
        }
            
        loadPlayer.SEchelonMissionFlag = false;

        if (loadPlayer.EchelonId == SCaptainEchelon)
        {
            loadPlayer.SCaptainFlag = true;
            return;
        }

        if (loadPlayer.EchelonId == SBrigadierEchelon)
        {
            loadPlayer.SBrigadierFlag = true;
        }
    }
    
    private void UpgradeEchelon(Response.PreLoadCard.LoadPlayer loadPlayer)
    {
        if (loadPlayer.EchelonId == GeneralEchelon)
        {
            loadPlayer.EchelonExp = 500;
            return;
        }
        
        loadPlayer.EchelonId += 1;
            
        var upgradeEchelonData = _context.EchelonSettings
            .First(x => x.EchelonId == loadPlayer.EchelonId);

        loadPlayer.EchelonExp = upgradeEchelonData.UpDefaultExp;
    }
    
    private void AdjustNegativeExp(Response.PreLoadCard.LoadPlayer loadPlayer)
    {
        if (loadPlayer.EchelonExp < 0)
        {
            loadPlayer.EchelonExp = 0;
        }
    }
    
    private void DowngradeEchelon(Response.PreLoadCard.LoadPlayer loadPlayer)
    {
        loadPlayer.EchelonId -= 1;
        
        var downgradeEchelonData = _context.EchelonSettings
            .First(x => x.EchelonId == loadPlayer.EchelonId);

        loadPlayer.EchelonExp = downgradeEchelonData.DownDefaultExp;
    }
}