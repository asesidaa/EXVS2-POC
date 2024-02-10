using ServerVanilla.Context.Battle;
using ServerVanilla.Context.Battle.Domain.Triad;
using ServerVanilla.Models.Cards;
using ServerVanilla.Models.Cards.Triad;
using ServerVanilla.Persistence;
using ServerVanilla.Utils;

namespace ServerVanilla.Command.SaveBattle.Triad;

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