using MudBlazor;
using WebUIOver.Shared.Dto.Response;

namespace WebUIOver.Client.Services;

public interface IResponseSnackService
{
    void ShowBasicResponseSnack(ISnackbar snackbar, BasicResponse result, string context = "");
}