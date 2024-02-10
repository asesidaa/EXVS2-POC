using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Common;
using WebUIVanilla.Shared.Dto.Enum;

namespace ServerVanilla.Handlers.Card.MobileSuit;

public record GetAllFavouriteMsCommand(string AccessCode, string ChipId) : IRequest<List<FavouriteMs>>;

public class GetAllFavouriteMsCommandHandler : IRequestHandler<GetAllFavouriteMsCommand, List<FavouriteMs>>
{
    private readonly ServerDbContext _context;
    
    public GetAllFavouriteMsCommandHandler(ServerDbContext context)
    {
        _context = context;
    }
    
    public Task<List<FavouriteMs>> Handle(GetAllFavouriteMsCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .Include(x => x.FavouriteMobileSuits)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var favouriteMsList = cardProfile.FavouriteMobileSuits
            .OrderBy(favouriteMs => favouriteMs.Id)
            .Select(favouriteMs =>
            {
                var bgmList = Array.Empty<uint>();

                if (favouriteMs.BgmSettings != string.Empty)
                {
                    bgmList = Array.ConvertAll(favouriteMs.BgmSettings.Split(','), Convert.ToUInt32);
                }
                
                return new FavouriteMs
                {
                    MsId = favouriteMs.MstMobileSuitId,
                    GaugeDesignId = favouriteMs.GaugeDesignId,
                    BgmPlayingMethod = (BgmPlayingMethod)favouriteMs.BgmPlayMethod,
                    BgmList = bgmList,
                    BattleNaviId = favouriteMs.BattleNavId,
                    BurstType = (BurstType) favouriteMs.BurstType,
                };
            })
            .ToList();
        
        return Task.FromResult(favouriteMsList);
    }
}