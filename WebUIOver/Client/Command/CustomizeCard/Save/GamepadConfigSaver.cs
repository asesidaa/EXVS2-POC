using System.Net.Http.Json;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Client.Services;
using WebUIOver.Client.Validator;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Resources;

namespace WebUIOver.Client.Command.CustomizeCard.Save;

public class GamepadConfigSaver : ICustomizeCardContentSaver
{
    private readonly HttpClient _httpClient;
    private readonly IResponseSnackService _responseSnackService;
    private readonly IStringLocalizer<Resource> _localizer;

    public GamepadConfigSaver(HttpClient httpClient, IResponseSnackService responseSnackService, IStringLocalizer<Resource> localizer)
    {
        _httpClient = httpClient;
        _responseSnackService = responseSnackService;
        _localizer = localizer;
    }

    public async Task Save(CustomizeCardContext customizeCardContext, ProgressContext progressContext, ISnackbar snackbar, Action stateHasChanged)
    {
        progressContext.HideGamepadConfigProgress = "visible";
        stateHasChanged.Invoke();
        
        var dto = new UpsertGamepadConfigRequest()
        {
            AccessCode = customizeCardContext.AccessCode,
            ChipId = customizeCardContext.ChipId,
            GamepadConfig = customizeCardContext.GamepadConfig
        };
        
        var response = await _httpClient.PostAsJsonAsync("/ui/gamepad/upsertGamepadConfig", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();
        
        _responseSnackService.ShowBasicResponseSnack(snackbar, result, _localizer["save_hint_gamepadconfig"]);

        progressContext.HideGamepadConfigProgress = "invisible";
        stateHasChanged.Invoke();
    }
}