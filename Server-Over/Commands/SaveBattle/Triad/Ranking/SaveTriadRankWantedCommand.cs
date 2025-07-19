using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Triad.Ranking;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.Triad.Ranking;

public class SaveTriadRankWantedCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;
    private readonly uint _year;
    private readonly uint _month;
    
    public SaveTriadRankWantedCommand(ServerDbContext context, uint year, uint month)
    {
        _context = context;
        _year = year;
        _month = month;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var isWin = battleResultContext.CommonDomain.IsWin;

        if (!isWin)
        {
            return;
        }
        
        var triadInfoDomain = battleResultContext.TriadInfoDomain;
        
        var defeatedCount = triadInfoDomain.Foes
            .Count(x => x.WantedMsFlag == 1 && x.DownNum > 0);
        
        if (defeatedCount == 0)
        {
            return;
        }
        
        var wantedRecord = _context.TriadWantedDefeatedCountDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile && x.Year == _year && x.Month == _month);
        
        if (wantedRecord is null)
        {
            var triadWantedRecord = new TriadWantedDefeatedCount()
            {
                CardProfile = cardProfile,
                Year = _year,
                Month = _month,
                DestroyCount = (uint) defeatedCount
            };
            
            _context.Add(triadWantedRecord);
            _context.SaveChanges();
            return;
        }

        wantedRecord.DestroyCount += (uint)defeatedCount;
        _context.SaveChanges();
    }
}