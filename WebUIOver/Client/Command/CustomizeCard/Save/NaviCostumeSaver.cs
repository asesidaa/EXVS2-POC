using System.Net.Http.Json;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Client.Services;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Resources;

namespace WebUIOver.Client.Command.CustomizeCard.Save;

public class NaviCostumeSaver : ICustomizeCardContentSaver
{
    private readonly HttpClient _httpClient;
    private readonly IResponseSnackService _responseSnackService;
    private readonly IStringLocalizer<Resource> _localizer;

    public NaviCostumeSaver(HttpClient httpClient, IResponseSnackService responseSnackService, IStringLocalizer<Resource> localizer)
    {
        _httpClient = httpClient;
        _responseSnackService = responseSnackService;
        _localizer = localizer;
    }

    public async Task Save(CustomizeCardContext customizeCardContext, ProgressContext progressContext, ISnackbar snackbar,
        Action stateHasChanged)
    {
        progressContext.HideNaviCostumeProgress = "visible";
        stateHasChanged.Invoke();

        var navis = customizeCardContext.NaviObservableCollection
            .Where(x => x.Navi != null)
            .Select(x => x.Navi)
            .ToList();

        var dto = new UpdateAllNaviCostumeRequest()
        {
            AccessCode = customizeCardContext.AccessCode,
            ChipId = customizeCardContext.ChipId,
            Navis = navis
        };
        
        var response = await _httpClient.PostAsJsonAsync("/ui/navi/updateAllNaviCostume", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        _responseSnackService.ShowBasicResponseSnack(snackbar, result, _localizer["save_hint_navcostume"]);

        progressContext.HideNaviCostumeProgress = "invisible";
        stateHasChanged.Invoke();
    }
}