using MudBlazor;
using WebUIOver.Client.Context.CustomizeCard;

namespace WebUIOver.Client.Command.CustomizeCard.Save;

public interface ICustomizeCardContentSaver
{
    Task Save(CustomizeCardContext customizeCardContext, ProgressContext progressContext, ISnackbar snackbar, Action stateHasChanged);
}