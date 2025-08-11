using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.Common;

public class SaveBurstTypeCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;

    public SaveBurstTypeCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var burstType = battleResultContext.BattleStatisticDomain.BurstType;
        uint winCount = battleResultContext.CommonDomain.IsWin ? (uint) 1 : (uint) 0;
        
        var burstTypeData = _context.PlayerBurstStatisticsDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile && x.BurstType == burstType);

        if (burstTypeData == null)
        {
            _context.Add(new PlayerBurstStatistics()
            {
                CardProfile = cardProfile,
                BurstType = burstType,
                TotalBattle = 1,
                TotalWin = winCount
            });

            _context.SaveChanges();
            
            return;
        }
        
        burstTypeData.TotalWin += winCount;
        burstTypeData.TotalBattle += 1;
    }
}