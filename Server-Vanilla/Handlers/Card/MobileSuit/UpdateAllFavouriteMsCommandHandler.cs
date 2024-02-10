using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using NuGet.Packaging;
using ServerVanilla.Models.Cards;
using ServerVanilla.Models.Cards.MobileSuit;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Common;
using WebUIVanilla.Shared.Dto.Request;
using WebUIVanilla.Shared.Dto.Response;
using WebUIVanilla.Shared.Exception;

namespace ServerVanilla.Handlers.Card.MobileSuit;

public record UpdateAllFavouriteMsCommand(UpdateAllFavouriteMsRequest Request) : IRequest<BasicResponse>;

public class UpdateAllFavouriteMsCommandHandler : IRequestHandler<UpdateAllFavouriteMsCommand, BasicResponse>
{
    private readonly ServerDbContext _context;
    
    public UpdateAllFavouriteMsCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpdateAllFavouriteMsCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;

        if (updateRequest.FavouriteMsList.Count > 6)
        {
            throw new InvalidRequestDataException("Favourite MS List should be having maximum length of 6");
        }
        
        var cardProfile = Queryable
            .FirstOrDefault<CardProfile>(_context.CardProfiles
                .Include(x => x.FavouriteMobileSuits)
                .Include(x => x.Navi)
                .Include(x => x.MobileSuits), x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var msSkills = cardProfile.MobileSuits;
        var navis = cardProfile.Navi;

        var favoriteMsGroups = updateRequest.FavouriteMsList
            .Select(ToFavouriteMsGroup(msSkills, navis))
            .ToList();

        cardProfile.FavouriteMobileSuits.Clear();
        cardProfile.FavouriteMobileSuits.AddRange(favoriteMsGroups);
        
        _context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }

    Func<FavouriteMs, FavouriteMobileSuit> ToFavouriteMsGroup(ICollection<MobileSuitUsage> msSkills, ICollection<Models.Cards.Navi.Navi> navis)
    {
        return favouriteMs =>
        {
            var targetMsUsage = msSkills.FirstOrDefault(msSkill => msSkill.MstMobileSuitId == favouriteMs.MsId);

            if (targetMsUsage is null)
            {
                msSkills.Add(new MobileSuitUsage
                {
                    MstMobileSuitId = favouriteMs.MsId,
                    CostumeId = 0,
                    MsUsedNum = 0,
                });
            }
            
            var targetNavi = navis.FirstOrDefault(navi => navi.GuestNavId == favouriteMs.BattleNaviId);

            if (favouriteMs.BattleNaviId is not 0 && targetNavi is null)
            {
                navis.Add(new Models.Cards.Navi.Navi()
                {
                    GuestNavId = favouriteMs.BattleNaviId,
                    GuestNavSettingFlag = false,
                    GuestNavRemains = 9999,
                    BattleNavSettingFlag = false,
                    BattleNavRemains = 9999,
                    GuestNavCostume = 0,
                    GuestNavFamiliarity = 0,
                    NewCostumeFlag = false,
                });
            }
            
            return new FavouriteMobileSuit()
            {
                MstMobileSuitId = favouriteMs.MsId,
                OpenSkillpoint = true,
                GaugeDesignId = favouriteMs.GaugeDesignId,
                BgmSettings = String.Join(",", favouriteMs.BgmList.ToArray()),
                BgmPlayMethod = (uint) favouriteMs.BgmPlayingMethod,
                BattleNavId = favouriteMs.BattleNaviId,
                BurstType = (uint) favouriteMs.BurstType
            };
        };
    }
}