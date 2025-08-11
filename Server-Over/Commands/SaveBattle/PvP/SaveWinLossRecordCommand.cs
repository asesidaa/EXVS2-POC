using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Enum;

namespace ServerOver.Commands.SaveBattle.PvP;

public class SaveWinLossRecordCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;

    public SaveWinLossRecordCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var commonDomain = battleResultContext.CommonDomain;
        var battleMode = commonDomain.BattleMode;
        
        if (battleMode == BattleModeConstant.FreeSolo)
        {
            return;
        }
        
        if (battleMode == BattleModeConstant.FreeTeam)
        {
            return;
        }
        
        var isWin = commonDomain.IsWin;

        var winCount = isWin ? 1u : 0u;
        var lossCount = isWin ? 0u : 1u;

        var winLossRecord = _context.WinLossRecordDbSet
            .First(x => x.CardProfile == cardProfile);

        winLossRecord.TotalWin += winCount;
        winLossRecord.TotalLose += lossCount;

        if (battleMode == BattleModeConstant.OfflineSolo)
        {
            winLossRecord.ShuffleWin += winCount;
            winLossRecord.ShuffleLose += lossCount;
            return;
        }

        if (battleMode == BattleModeConstant.OfflineTeam)
        {
            winLossRecord.TeamWin += winCount;
            winLossRecord.TeamLose += lossCount;
            return;
        }

        if (battleMode == BattleModeConstant.ClassMatchSolo)
        {
            winLossRecord.ClassSoloWin += winCount;
            winLossRecord.ClassSoloLose += lossCount;
            return;
        }
        
        if (battleMode == BattleModeConstant.ClassMatchTeam)
        {
            winLossRecord.ClassTeamWin += winCount;
            winLossRecord.ClassTeamLose += lossCount;
            return;
        }
        
        if (battleMode == BattleModeConstant.FesSolo || battleMode == BattleModeConstant.FesTeam)
        {
            winLossRecord.FesWin += winCount;
            winLossRecord.FesLose += lossCount;
            return;
        }
    }
}