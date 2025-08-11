using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOVer.Shared.Dto.Common;

namespace WebUIOver.Client.Command.CustomizeCard.Fill;

public class FavouriteMobileSuitFiller : ICustomizeCardContextFiller
{
    private HttpClient _httpClient;

    public FavouriteMobileSuitFiller(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Fill(CustomizeCardContext customizeCardContext)
    {
        var favouriteResult = await _httpClient.GetFromJsonAsync<List<FavouriteMs>>($"/ui/mobileSuit/getAllFavouriteMs/{customizeCardContext.AccessCode}/{customizeCardContext.ChipId}");
        favouriteResult.ThrowIfNull();

        customizeCardContext.FavouriteMsCollection = new ObservableCollection<FavouriteMs>(favouriteResult);
    }
}