using System.Net.Http.Json;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Client.Services;
using WebUIOver.Client.Validator;
using WebUIOver.Shared.Dto.Request.Sticker;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Resources;

namespace WebUIOver.Client.Command.CustomizeCard.Save;

public class DefaultStickerSettingSaver : ICustomizeCardContentSaver
{
    private readonly HttpClient _httpClient;
    private readonly INameValidator _nameValidator;
    private readonly IResponseSnackService _responseSnackService;
    private readonly IStringLocalizer<Resource> _localizer;

    public DefaultStickerSettingSaver(HttpClient httpClient, INameValidator nameValidator, IResponseSnackService responseSnackService, IStringLocalizer<Resource> localizer)
    {
        _httpClient = httpClient;
        _nameValidator = nameValidator;
        _responseSnackService = responseSnackService;
        _localizer = localizer;
    }

    public async Task Save(CustomizeCardContext customizeCardContext, ProgressContext progressContext, ISnackbar snackbar, Action stateHasChanged)
    {
        progressContext.HideDefaultStickerProgress = "visible";
        stateHasChanged.Invoke();
        
        var dto = new UpdateDefaultStickerRequest()
        {
            AccessCode = customizeCardContext.AccessCode,
            ChipId = customizeCardContext.ChipId,
            StickerDto = customizeCardContext.DefaultStickerSetting
        };
        
        var response = await _httpClient.PostAsJsonAsync("/ui/sticker/updateDefaultSticker", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();
        
        _responseSnackService.ShowBasicResponseSnack(snackbar, result, _localizer["save_hint_default_sticker"]);

        progressContext.HideDefaultStickerProgress = "invisible";
        stateHasChanged.Invoke();
    }
}