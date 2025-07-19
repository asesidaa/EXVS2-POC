using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.MobileSuit;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.Common;

public class SaveMobileSuitMasteryCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;

    public SaveMobileSuitMasteryCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var masteryMsId = battleResultContext.MobileSuitMasteryDomain.MasteryMobileSuitId;
        var masteryMs = _context.MobileSuitUsageDbSet
            .FirstOrDefault(x =>
                x.CardProfile == cardProfile &&
                x.MstMobileSuitId == masteryMsId
            );

        if (masteryMs is null)
        {
            _context.Add(new MobileSuitUsage()
            {
                CardProfile = cardProfile,
                MstMobileSuitId = masteryMsId,
                MsUsedNum = 1
            });
            
            _context.SaveChanges();
            
            return;
        }

        masteryMs.MsUsedNum += 1;
    }
}