using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerOver.Mapper.Card.Titles;
using ServerOver.Persistence;
using ServerOver.Utils;
using WebUIOVer.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Enum;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.MobileSuit;

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
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile is null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }
        
        var favouriteMsList = _context.FavouriteMobileSuitDbSet
            .Include(x => x.MobileSuitDefaultTitle)
            .Include(x => x.MobileSuitTriadTitle)
            .Include(x => x.MobileSuitClassMatchTitle)
            .Where(x => x.CardProfile == cardProfile)
            .OrderBy(favouriteMs => favouriteMs.Id)
            .ToList()
            .Select(favouriteMs => new FavouriteMs
                {
                    MsId = favouriteMs.MstMobileSuitId,
                    GaugeDesignId = favouriteMs.GaugeDesignId,
                    BgmPlayingMethod = (BgmPlayingMethod)favouriteMs.BgmPlayMethod,
                    BgmList = Array.ConvertAll(ArrayUtil.FromString(favouriteMs.BgmSettings), Convert.ToUInt32),
                    BattleNaviId = favouriteMs.BattleNavId,
                    BurstType = (BurstType) favouriteMs.BurstType,
                    DefaultTitle = favouriteMs.MobileSuitDefaultTitle.ToTitle(),
                    TriadTitle = favouriteMs.MobileSuitTriadTitle.ToTitle(),
                    ClassMatchTitle = favouriteMs.MobileSuitClassMatchTitle.ToTitle(),
                }
            )
            .ToList();
        
        return Task.FromResult(favouriteMsList);
    }
}