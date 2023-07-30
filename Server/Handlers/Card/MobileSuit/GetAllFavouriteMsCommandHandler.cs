using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Enum;
using WebUI.Shared.Exception;

namespace Server.Handlers.Card.MobileSuit;

public record GetAllFavouriteMsCommand(String AccessCode, String ChipId) : IRequest<List<FavouriteMs>>;

public class GetAllFavouriteMsCommandHandler : IRequestHandler<GetAllFavouriteMsCommand, List<FavouriteMs>>
{
    private readonly ServerDbContext context;
    
    public GetAllFavouriteMsCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }
    
    public Task<List<FavouriteMs>> Handle(GetAllFavouriteMsCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var user = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
        
        if (user is null)
        {
            throw new InvalidCardDataException("Card Data is invalid");
        }

        var favouriteMsList = user.FavoriteMobileSuits
            .Select(favouriteMs => new FavouriteMs
            {
                MsId = favouriteMs.MstMobileSuitId,
                GaugeDesignId = favouriteMs.GaugeDesignId,
                BgmPlayingMethod = (BgmPlayingMethod) favouriteMs.BgmPlayMethod,
                BgmList = favouriteMs.BgmSettings,
                BattleNaviId = favouriteMs.BattleNavId,
                BurstType = (BurstType) favouriteMs.BurstType,
                DefaultTitle = new Title
                {
                    TextId = favouriteMs.DefaultTitleCustomize.TitleTextId,
                    EffectId = favouriteMs.DefaultTitleCustomize.TitleEffectId,
                    OrnamentId = favouriteMs.DefaultTitleCustomize.TitleOrnamentId,
                    BackgroundPartsId = favouriteMs.DefaultTitleCustomize.TitleBackgroundPartsId
                },
                TriadTitle = new Title
                {
                    TextId = favouriteMs.TriadTitleCustomize.TitleTextId,
                    EffectId = favouriteMs.TriadTitleCustomize.TitleEffectId,
                    OrnamentId = favouriteMs.TriadTitleCustomize.TitleOrnamentId,
                    BackgroundPartsId = favouriteMs.TriadTitleCustomize.TitleBackgroundPartsId
                },
                RankingTitle = new Title
                {
                    TextId = favouriteMs.RankMatchTitleCustomize.TitleTextId,
                    EffectId = favouriteMs.RankMatchTitleCustomize.TitleEffectId,
                    OrnamentId = favouriteMs.RankMatchTitleCustomize.TitleOrnamentId,
                    BackgroundPartsId = favouriteMs.RankMatchTitleCustomize.TitleBackgroundPartsId
                }
            })
            .ToList();
        
        return Task.FromResult(favouriteMsList);
    }
}