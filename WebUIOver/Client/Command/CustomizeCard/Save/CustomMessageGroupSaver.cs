using System.Net.Http.Json;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Client.Services;
using WebUIOver.Client.Validator;
using WebUIOver.Shared.Dto.Message;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Resources;

namespace WebUIOver.Client.Command.CustomizeCard.Save;

public class CustomMessageGroupSaver : ICustomizeCardContentSaver
{
    private readonly HttpClient _httpClient;
    private readonly INameValidator _nameValidator;
    private readonly IResponseSnackService _responseSnackService;
    private readonly IStringLocalizer<Resource> _localizer;

    public CustomMessageGroupSaver(HttpClient httpClient, INameValidator nameValidator, IResponseSnackService responseSnackService, IStringLocalizer<Resource> localizer)
    {
        _httpClient = httpClient;
        _nameValidator = nameValidator;
        _responseSnackService = responseSnackService;
        _localizer = localizer;
    }

    public async Task Save(CustomizeCardContext customizeCardContext, ProgressContext progressContext, ISnackbar snackbar,
        Action stateHasChanged)
    {
        var customMessageGroupSetting = customizeCardContext.CustomMessageGroupSetting;
        
        var allMessageValid = AllMessageValid(customMessageGroupSetting.StartGroup)
                              && AllMessageValid(customMessageGroupSetting.InBattleGroup)
                              && AllMessageValid(customMessageGroupSetting.ResultGroup)
                              && AllMessageValid(customMessageGroupSetting.OnlineShuffleStartGroup)
                              && AllMessageValid(customMessageGroupSetting.OnlineShuffleInBattleGroup)
                              && AllMessageValid(customMessageGroupSetting.OnlineShuffleResultGroup);
        
        if (!allMessageValid)
        {
            _responseSnackService.ShowBasicResponseSnack(snackbar, new BasicResponse { Success = false }, _localizer["save_hint_commconfig"]);
            return;
        }
        
        progressContext.HideCommunicationMessageProgress = "visible";
        stateHasChanged.Invoke();
        
        var dto = new UpsertCustomMessagesRequest()
        {
            AccessCode = customizeCardContext.AccessCode,
            ChipId = customizeCardContext.ChipId,
            MessageSetting = customMessageGroupSetting
        };

        var response = await _httpClient.PostAsJsonAsync("/ui/message/upsertCustomMessages", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();;

        _responseSnackService.ShowBasicResponseSnack(snackbar, result, _localizer["save_hint_commconfig"]);

        progressContext.HideCommunicationMessageProgress = "invisible";
        stateHasChanged.Invoke();
    }

    bool AllMessageValid(CustomMessageGroup customMessageGroup)
    {
        return _nameValidator.ValidateCustomizeMessage(customMessageGroup.UpMessage.MessageText) is null
               && _nameValidator.ValidateCustomizeMessage(customMessageGroup.DownMessage.MessageText) is null
               && _nameValidator.ValidateCustomizeMessage(customMessageGroup.LeftMessage.MessageText) is null
               && _nameValidator.ValidateCustomizeMessage(customMessageGroup.RightMessage.MessageText) is null;
    }
}