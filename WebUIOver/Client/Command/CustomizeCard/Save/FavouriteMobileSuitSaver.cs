using System.Net.Http.Json;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Client.Services;
using WebUIOver.Client.Validator;
using WebUIOVer.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Resources;

namespace WebUIOver.Client.Command.CustomizeCard.Save;

public class FavouriteMobileSuitSaver : ICustomizeCardContentSaver
{
    private readonly HttpClient _httpClient;
    private readonly INameValidator _nameValidator;
    private readonly IResponseSnackService _responseSnackService;
    private readonly IStringLocalizer<Resource> _localizer;

    public FavouriteMobileSuitSaver(HttpClient httpClient, INameValidator nameValidator, IResponseSnackService responseSnackService, IStringLocalizer<Resource> localizer)
    {
        _httpClient = httpClient;
        _nameValidator = nameValidator;
        _responseSnackService = responseSnackService;
        _localizer = localizer;
    }

    public async Task Save(CustomizeCardContext customizeCardContext, ProgressContext progressContext, ISnackbar snackbar,
        Action stateHasChanged)
    {
        foreach (FavouriteMs favouriteMs in customizeCardContext.FavouriteMsCollection)
        {
            if (_nameValidator.ValidateCustomizeTitle(favouriteMs.DefaultTitle.CustomText) is not null)
            {
                _responseSnackService.ShowBasicResponseSnack(snackbar, new BasicResponse { Success = false }, _localizer["save_hint_favms"]);
                return;
            }
        
            if (_nameValidator.ValidateCustomizeTitle(favouriteMs.TriadTitle.CustomText) is not null)
            {
                _responseSnackService.ShowBasicResponseSnack(snackbar, new BasicResponse { Success = false }, _localizer["save_hint_favms"]);
                return;
            }
        
            if (_nameValidator.ValidateCustomizeTitle(favouriteMs.ClassMatchTitle.CustomText) is not null)
            {
                _responseSnackService.ShowBasicResponseSnack(snackbar, new BasicResponse { Success = false }, _localizer["save_hint_favms"]);
                return;
            }
        }
        
        progressContext.HideFavMsProgress = "visible";
        stateHasChanged.Invoke();

        var dto = new UpdateAllFavouriteMsRequest()
        {
            AccessCode = customizeCardContext.AccessCode,
            ChipId = customizeCardContext.ChipId,
            FavouriteMsList = customizeCardContext.FavouriteMsCollection.ToList()
        };
        
        var response = await _httpClient.PostAsJsonAsync("/ui/mobileSuit/updateAllFavouriteMs", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        _responseSnackService.ShowBasicResponseSnack(snackbar, result, _localizer["save_hint_favms"]);

        progressContext.HideFavMsProgress = "invisible";
        stateHasChanged.Invoke();
    }
}