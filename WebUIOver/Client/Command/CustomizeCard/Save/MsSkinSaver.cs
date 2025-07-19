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

public class MsSkinSaver : ICustomizeCardContentSaver
{
    private readonly HttpClient _httpClient;
    private readonly IResponseSnackService _responseSnackService;
    private readonly IStringLocalizer<Resource> _localizer;

    public MsSkinSaver(HttpClient httpClient, IResponseSnackService responseSnackService, IStringLocalizer<Resource> localizer)
    {
        _httpClient = httpClient;
        _responseSnackService = responseSnackService;
        _localizer = localizer;
    }

    public async Task Save(CustomizeCardContext customizeCardContext, ProgressContext progressContext, ISnackbar snackbar,
        Action stateHasChanged)
    {
        progressContext.HideMsSkinProgress = "visible";
        stateHasChanged.Invoke();
        
        var newSkillGroup = customizeCardContext.AlternativeSkinMobileSuitsSkillGroups
            .Where(x => x.SkillGroup != null)
            .Select(x => x.SkillGroup)
            .ToList();
        
        var dto = new UpdateAllMsCostumeSkinRequest()
        {
            AccessCode = customizeCardContext.AccessCode,
            ChipId = customizeCardContext.ChipId,
            MsSkillGroup = newSkillGroup
        };

        var response = await _httpClient.PostAsJsonAsync("/ui/mobileSuit/updateAllMsSkin", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        _responseSnackService.ShowBasicResponseSnack(snackbar, result, _localizer["save_hint_msskin"]);

        progressContext.HideMsSkinProgress = "invisible";
        stateHasChanged.Invoke();
    }
}