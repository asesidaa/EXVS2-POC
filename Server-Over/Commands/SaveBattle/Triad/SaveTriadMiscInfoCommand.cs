using ServerOver.Context.Battle;
using ServerOver.Context.Battle.Domain.Triad;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Triad;
using ServerOver.Persistence;
using ServerOver.Utils;

namespace ServerOver.Commands.SaveBattle.Triad;

public class SaveTriadMiscInfoCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;

    public SaveTriadMiscInfoCommand(ServerDbContext context)
    {
        _context = context;
    }
    
    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var commonDomain = battleResultContext.CommonDomain;
        var triadInfoDomain = battleResultContext.TriadInfoDomain;
        
        var triadMiscInfo = _context.TriadMiscInfoDbSet
            .First(x => x.CardProfile == cardProfile);

        triadMiscInfo.TotalTriadScore += triadInfoDomain.SceneScore;
        triadMiscInfo.TotalTriadScenePlayNum += 1;
        
        if (commonDomain.IsWin)
        {
            triadMiscInfo.TotalTriadWantedDefeatNum += triadInfoDomain.TotalWantedDefeatNum;
        }

        HandleUnlockRibbon(triadInfoDomain, triadMiscInfo);
    }

    void HandleUnlockRibbon(TriadInfoDomain triadInfoDomain, TriadMiscInfo triadMiscInfo)
    {
        var releasedCpuRibbons = triadInfoDomain.ReleasedRibbonIds;

        if (releasedCpuRibbons is null)
        {
            return;
        }

        var currentCpuRibbons = ArrayUtil.FromString(triadMiscInfo.CpuRibbons)
            .ToList();

        releasedCpuRibbons
            .ToList()
            .ForEach(releaseCpuRibbon =>
            {
                if (currentCpuRibbons.Contains(releaseCpuRibbon))
                {
                    return;
                }

                currentCpuRibbons.Add(releaseCpuRibbon);
            });

        triadMiscInfo.CpuRibbons = String.Join(",", currentCpuRibbons.ToArray());
    }
}