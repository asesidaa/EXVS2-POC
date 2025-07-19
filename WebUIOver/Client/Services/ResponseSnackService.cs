using Microsoft.Extensions.Localization;
using MudBlazor;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Resources;

namespace WebUIOver.Client.Services;

public class ResponseSnackService : IResponseSnackService
{
    private readonly IStringLocalizer<Resource> _localizer;

    public ResponseSnackService(IStringLocalizer<Resource> localizer)
    {
        _localizer = localizer;
    }

    public void ShowBasicResponseSnack(ISnackbar snackbar, BasicResponse result, string context = "")
    {
        if (result.Success)
            snackbar.Add($"{_localizer["save_hint_update"]}{context}{_localizer["save_hint_successful"]}", Severity.Success);
        else
            snackbar.Add($"{_localizer["save_hint_update"]}{context}{_localizer["save_hint_failed"]}", Severity.Error);
    }
}