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

public class NaviProfileSaver : ICustomizeCardContentSaver
{
    private readonly HttpClient _httpClient;
    private readonly IResponseSnackService _responseSnackService;
    private readonly IStringLocalizer<Resource> _localizer;

    public NaviProfileSaver(HttpClient httpClient, IResponseSnackService responseSnackService, IStringLocalizer<Resource> localizer)
    {
        _httpClient = httpClient;
        _responseSnackService = responseSnackService;
        _localizer = localizer;
    }

    public async Task Save(CustomizeCardContext customizeCardContext, ProgressContext progressContext, ISnackbar snackbar,
        Action stateHasChanged)
    {
        progressContext.HideNaviProgress = "visible";
        stateHasChanged.Invoke();

        var dto = new UpsertNaviProfileRequest()
        {
            AccessCode = customizeCardContext.AccessCode,
            ChipId = customizeCardContext.ChipId,
            DefaultBattleNaviId = customizeCardContext.NaviProfile.DefaultBattleNaviId,
            DefaultUiNaviId = customizeCardContext.NaviProfile.DefaultUiNaviId,
            BattleNavAdviseFlag = customizeCardContext.NaviProfile.BattleNavAdviseFlag,
            BattleNavNotifyFlag = customizeCardContext.NaviProfile.BattleNavNotifyFlag
        };

        var response = await _httpClient.PostAsJsonAsync("/ui/navi/upsertNaviProfile", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        _responseSnackService.ShowBasicResponseSnack(snackbar, result, _localizer["save_hint_navinfo"]);

        progressContext.HideNaviProgress = "invisible";
        stateHasChanged.Invoke();
    }
}