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

public class TeamSaver : ICustomizeCardContentSaver
{
    private readonly HttpClient _httpClient;
    private readonly INameValidator _nameValidator;
    private readonly IResponseSnackService _responseSnackService;
    private readonly IStringLocalizer<Resource> _localizer;

    public TeamSaver(HttpClient httpClient, INameValidator nameValidator, IResponseSnackService responseSnackService, IStringLocalizer<Resource> localizer)
    {
        _httpClient = httpClient;
        _nameValidator = nameValidator;
        _responseSnackService = responseSnackService;
        _localizer = localizer;
    }
    
    public async Task Save(CustomizeCardContext customizeCardContext, ProgressContext progressContext, ISnackbar snackbar, Action stateHasChanged)
    {
        var errorCount = 0;
        var onlineTeamCount = 0;
        
        customizeCardContext.TeamResponse.Teams.ForEach(team =>
        {
            if (_nameValidator.ValidatePvPTeamName(team.Name) is not null)
            {
                errorCount++;
            }

            if (team.OnlineTag)
            {
                onlineTeamCount++;
            }
        });
        
        if (errorCount > 0)
        {
            _responseSnackService.ShowBasicResponseSnack(snackbar, new BasicResponse { Success = false }, _localizer["save_hint_team"]);
            return;
        }

        if (onlineTeamCount > 5)
        {
            snackbar.Add(_localizer["save_hint_team_online"], Severity.Error);
            return;
        }
        
        progressContext.HideTeamTagsProgress = "visible";
        stateHasChanged.Invoke();
        
        var dto = new UpsertTeamsRequest()
        {
            AccessCode = customizeCardContext.AccessCode,
            ChipId = customizeCardContext.ChipId,
            Teams = customizeCardContext.TeamResponse.Teams
        };
        
        var response = await _httpClient.PostAsJsonAsync("/ui/team/upsertTeams", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();
        
        _responseSnackService.ShowBasicResponseSnack(snackbar, result, _localizer["save_hint_team"]);

        progressContext.HideTeamTagsProgress = "invisible";
        stateHasChanged.Invoke();
    }
}