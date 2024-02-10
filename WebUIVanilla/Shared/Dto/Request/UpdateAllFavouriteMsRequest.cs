using WebUIVanilla.Shared.Dto.Common;

namespace WebUIVanilla.Shared.Dto.Request;

public class UpdateAllFavouriteMsRequest : BasicCardRequest
{
    public List<FavouriteMs> FavouriteMsList { get; set; } = new();
}