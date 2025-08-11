using WebUIOVer.Shared.Dto.Common;

namespace WebUIOver.Shared.Dto.Request;

public class UpdateAllFavouriteMsRequest : BasicCardRequest
{
    public List<FavouriteMs> FavouriteMsList { get; set; } = new();
}