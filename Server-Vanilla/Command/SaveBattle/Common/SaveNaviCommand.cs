using ServerVanilla.Context.Battle;
using ServerVanilla.Models.Cards;
using ServerVanilla.Persistence;

namespace ServerVanilla.Command.SaveBattle.Common;

public class SaveNaviCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;

    public SaveNaviCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        if (battleResultContext.NaviDomain.GuestNavId == battleResultContext.NaviDomain.BattleNavId)
        {
            UpdateForSameNavi(cardProfile, battleResultContext);
            return;
        }

        UpdateForDifferentNavis(cardProfile, battleResultContext);
    }
    
    void UpdateForSameNavi(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var navi = _context.NaviDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile && x.GuestNavId == battleResultContext.NaviDomain.GuestNavId);

        if (navi is null)
        {
            return;
        }

        navi.GuestNavFamiliarity += battleResultContext.NaviDomain.GuestNavFamiliarityIncrement;
        navi.GuestNavFamiliarity += battleResultContext.NaviDomain.BattleNavFamiliarityIncrement;
    }
    
    void UpdateForDifferentNavis(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var guestNavi = _context.NaviDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile && x.GuestNavId == battleResultContext.NaviDomain.GuestNavId);

        if (guestNavi is not null)
        {
            guestNavi.GuestNavFamiliarity += battleResultContext.NaviDomain.GuestNavFamiliarityIncrement;
        }

        var battleNavi = _context.NaviDbSet
            .FirstOrDefault(x =>
                x.CardProfile == cardProfile && x.GuestNavId == battleResultContext.NaviDomain.BattleNavId);

        if (battleNavi is not null)
        {
            battleNavi.GuestNavFamiliarity += battleResultContext.NaviDomain.BattleNavFamiliarityIncrement;
        }
    }
}