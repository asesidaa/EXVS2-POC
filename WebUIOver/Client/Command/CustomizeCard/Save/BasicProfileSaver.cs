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

public class BasicProfileSaver : ICustomizeCardContentSaver
{
    private readonly HttpClient _httpClient;
    private readonly INameValidator _nameValidator;
    private readonly IResponseSnackService _responseSnackService;
    private readonly IStringLocalizer<Resource> _localizer;

    public BasicProfileSaver(HttpClient httpClient, INameValidator nameValidator, IResponseSnackService responseSnackService, IStringLocalizer<Resource> localizer)
    {
        _httpClient = httpClient;
        _nameValidator = nameValidator;
        _responseSnackService = responseSnackService;
        _localizer = localizer;
    }

    public async Task Save(CustomizeCardContext customizeCardContext, ProgressContext progressContext, ISnackbar snackbar, Action stateHasChanged)
    {
        if (_nameValidator.ValidatePlayerName(customizeCardContext.BasicProfile.UserName) is not null)
        {
            _responseSnackService.ShowBasicResponseSnack(snackbar, new BasicResponse { Success = false }, _localizer["save_hint_cardinfo"]);
            return;
        }
        
        if (_nameValidator.ValidateCustomizeTitle(customizeCardContext.BasicProfile.DefaultTitle.CustomText) is not null)
        {
            _responseSnackService.ShowBasicResponseSnack(snackbar, new BasicResponse { Success = false }, _localizer["save_hint_cardinfo"]);
            return;
        }
        
        if (_nameValidator.ValidateCustomizeTitle(customizeCardContext.BasicProfile.TriadTitle.CustomText) is not null)
        {
            _responseSnackService.ShowBasicResponseSnack(snackbar, new BasicResponse { Success = false }, _localizer["save_hint_cardinfo"]);
            return;
        }
        
        if (_nameValidator.ValidateCustomizeTitle(customizeCardContext.BasicProfile.ClassMatchTitle.CustomText) is not null)
        {
            _responseSnackService.ShowBasicResponseSnack(snackbar, new BasicResponse { Success = false }, _localizer["save_hint_cardinfo"]);
            return;
        }

        progressContext.HideProfileProgress = "visible";
        stateHasChanged.Invoke();
        
        var dto = new UpdateBasicProfileRequest()
        {
            AccessCode = customizeCardContext.AccessCode,
            ChipId = customizeCardContext.ChipId,
            BasicProfile = customizeCardContext.BasicProfile
        };
        
        var response = await _httpClient.PostAsJsonAsync("/ui/card/updateBasicProfile", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();
        
        _responseSnackService.ShowBasicResponseSnack(snackbar, result, _localizer["save_hint_cardinfo"]);

        progressContext.HideProfileProgress = "invisible";
        stateHasChanged.Invoke();
    }
}