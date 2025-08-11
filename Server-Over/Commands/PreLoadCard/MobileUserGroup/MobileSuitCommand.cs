using Microsoft.EntityFrameworkCore;
using nue.protocol.exvs;
using ServerOver.Mapper.Card.Titles.MobileSuit;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.MobileSuit;
using ServerOver.Persistence;
using ServerOver.Utils;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public class MobileSuitCommand : IPreLoadMobileUserGroupCommand
{
    private readonly ServerDbContext _context;

    public MobileSuitCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.MobileUserGroup mobileUserGroup)
    {
        var favouriteMobileSuits = _context.FavouriteMobileSuitDbSet
            .Where(x => x.CardProfile == cardProfile)
            .Include(x => x.MobileSuitDefaultTitle)
            .Include(x => x.MobileSuitTriadTitle)
            .Include(x => x.MobileSuitClassMatchTitle)
            .OrderBy(favouriteMs => favouriteMs.Id)
            .ToList();
        
        var mobileSuitUsages = _context.MobileSuitUsageDbSet
            .Where(x => x.CardProfile == cardProfile)
            .OrderBy(mobileSuit => mobileSuit.MstMobileSuitId)
            .ToList();
        
        FillFavouriteMobileSuits(mobileUserGroup, favouriteMobileSuits, mobileSuitUsages);
        FillMobileSuitSkins(mobileUserGroup, mobileSuitUsages);
    }

    void FillFavouriteMobileSuits(Response.PreLoadCard.MobileUserGroup mobileUserGroup, List<FavouriteMobileSuit> favouriteMobileSuits, List<MobileSuitUsage> mobileSuitUsages)
    {
        favouriteMobileSuits.ForEach(favouriteMs =>
        {
            var bgmSettings = ArrayUtil.FromString(favouriteMs.BgmSettings);
            
            var favouriteMsGroup = new Response.PreLoadCard.MobileUserGroup.FavoriteMSGroup();
            
            FillGenericFavouriteMobileSuitInfo(favouriteMsGroup, favouriteMs, bgmSettings);
            FillFavouriteMobileSuitUsage(mobileSuitUsages, favouriteMs, favouriteMsGroup);
            FillMobileSuitTitle(favouriteMsGroup, favouriteMs);

            mobileUserGroup.FavoriteMobileSuits.Add(favouriteMsGroup);
        });
    }

    void FillGenericFavouriteMobileSuitInfo(Response.PreLoadCard.MobileUserGroup.FavoriteMSGroup favouriteMsGroup, FavouriteMobileSuit favouriteMs,
        uint[] bgmSettings)
    {
        favouriteMsGroup.MstMobileSuitId = favouriteMs.MstMobileSuitId;
        favouriteMsGroup.MsUsedNum = 0;
        favouriteMsGroup.OpenSkillpoint = favouriteMs.OpenSkillpoint;
        favouriteMsGroup.GaugeDesignId = favouriteMs.GaugeDesignId;
        favouriteMsGroup.BgmSettings = bgmSettings;
        favouriteMsGroup.BgmPlayMethod = favouriteMs.BgmPlayMethod;
        favouriteMsGroup.BattleNavId = favouriteMs.BattleNavId;
        favouriteMsGroup.BurstType = favouriteMs.BurstType;
    }

    void FillFavouriteMobileSuitUsage(List<MobileSuitUsage> mobileSuitUsages, FavouriteMobileSuit favouriteMs,
        Response.PreLoadCard.MobileUserGroup.FavoriteMSGroup favouriteMsGroup)
    {
        var msUsage = mobileSuitUsages
            .FirstOrDefault(ms => ms.MstMobileSuitId == favouriteMs.MstMobileSuitId);

        if (msUsage is not null)
        {
            favouriteMsGroup.MsUsedNum = msUsage.MsUsedNum;
        }
    }

    void FillMobileSuitTitle(Response.PreLoadCard.MobileUserGroup.FavoriteMSGroup favouriteMsGroup, FavouriteMobileSuit favouriteMs)
    {
        favouriteMsGroup.DefaultTitleCustomize = favouriteMs.MobileSuitDefaultTitle.ToTitleCustomize();
        favouriteMsGroup.ClassMatchTitleCustomize = favouriteMs.MobileSuitClassMatchTitle.ToTitleCustomize();
        favouriteMsGroup.TriadTitleCustomize = favouriteMs.MobileSuitTriadTitle.ToTitleCustomize();
    }

    void FillMobileSuitSkins(Response.PreLoadCard.MobileUserGroup mobileUserGroup, List<MobileSuitUsage> mobileSuitUsages)
    {
        mobileSuitUsages
            .Where(mobileSuit => mobileSuit.SkinId != 0)
            .ToList()
            .ForEach(msSkin => mobileUserGroup.skin_change_mobilesuits.Add(
                new Response.PreLoadCard.MobileUserGroup.SkinChangeMobilesuits()
                {
                    MstMobileSuitId = msSkin.MstMobileSuitId,
                    SkinId = msSkin.SkinId
                }));
    }
}