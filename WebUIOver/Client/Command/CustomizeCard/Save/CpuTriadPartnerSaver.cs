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

public class CpuTriadPartnerSaver : ICustomizeCardContentSaver
{
    private readonly HttpClient _httpClient;
    private readonly INameValidator _nameValidator;
    private readonly IResponseSnackService _responseSnackService;
    private readonly IStringLocalizer<Resource> _localizer;

    public CpuTriadPartnerSaver(HttpClient httpClient, INameValidator nameValidator, IResponseSnackService responseSnackService, IStringLocalizer<Resource> localizer)
    {
        _httpClient = httpClient;
        _nameValidator = nameValidator;
        _responseSnackService = responseSnackService;
        _localizer = localizer;
    }

    public async Task Save(CustomizeCardContext customizeCardContext, ProgressContext progressContext, ISnackbar snackbar, Action stateHasChanged)
    {
        var cpuTriadPartner = customizeCardContext.CpuTriadPartner;
        
        if (_nameValidator.ValidateTeamName(cpuTriadPartner.TriadTeamName) is not null)
        {
            _responseSnackService.ShowBasicResponseSnack(snackbar, new BasicResponse { Success = false }, _localizer["save_hint_triadcpupartner"]);
            return;
        }
        
        int totalLevel = cpuTriadPartner.ArmorLevel + cpuTriadPartner.ShootAttackLevel + cpuTriadPartner.InfightAttackLevel
                         + cpuTriadPartner.BoosterLevel + cpuTriadPartner.ExGaugeLevel + cpuTriadPartner.AiLevel;

        if (totalLevel > 500)
        {
            snackbar.Add(_localizer["save_hint_cpulimit"], Severity.Warning);
            return;
        }

        progressContext.HideTriadPartnerProgress = "visible";
        stateHasChanged.Invoke();
        
        var dto = new UpdateCpuTriadPartnerRequest()
        {
            AccessCode = customizeCardContext.AccessCode,
            ChipId = customizeCardContext.ChipId,
            CpuTriadPartner = cpuTriadPartner
        };
        
        var response = await _httpClient.PostAsJsonAsync("/ui/triad/updateCpuTriadPartner", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();
        
        _responseSnackService.ShowBasicResponseSnack(snackbar, result, _localizer["save_hint_triadcpupartner"]);

        progressContext.HideTriadPartnerProgress = "invisible";
        stateHasChanged.Invoke();
    }
}