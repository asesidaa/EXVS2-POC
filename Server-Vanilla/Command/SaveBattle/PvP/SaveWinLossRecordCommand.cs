using ServerVanilla.Context.Battle;
using ServerVanilla.Models.Cards;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Enum;

namespace ServerVanilla.Command.SaveBattle.PvP;

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
        
        var isWin = commonDomain.IsWin;

        var winCount = isWin ? 1u : 0u;
        var lossCount = isWin ? 0u : 1u;

        var battleProfile = _context.BattleProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        battleProfile.TotalWin += winCount;
        battleProfile.TotalLose += lossCount;

        if (battleMode == BattleModeConstant.OfflineSolo)
        {
            battleProfile.ShuffleWin += winCount;
            battleProfile.ShuffleLose += lossCount;
            return;
        }

        if (battleMode == BattleModeConstant.OfflineTeam)
        {
            battleProfile.TeamWin += winCount;
            battleProfile.TeamLose += lossCount;
            return;
        }
    }
}