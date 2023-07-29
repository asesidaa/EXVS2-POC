using WebUI.Shared.Dto.Common;

namespace WebUI.Shared.Dto.Request;

public class UpdateAllFavouriteMsRequest : BasicCardRequest
{
    public List<FavouriteMs> FavouriteMsList { get; set; } = new();
}