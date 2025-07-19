using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Throw;
using WebUIOver.Shared.Dto.Response;

namespace WebUIOver.Client.Pages;

public partial class SingularCardMenu
{
    [Parameter]
    public string ChipId { get; set; } = string.Empty;
    
    [Parameter]
    public string AccessCode { get; set; } = string.Empty;
    
    [Inject]
    private IJSRuntime? _jsRuntime { get; set; }
    
    private BareboneCardProfile _cardProfile = new BareboneCardProfile()
    {
        CardId = 0,
        ChipId = "",
        UserName = ""
    };
    
    private string? errorMessage = null;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        errorMessage = null;

        AccessCode = await _jsRuntime.InvokeAsync<string>("accessCode.get");
        
        if (string.IsNullOrEmpty(AccessCode))
        {
            Snackbar.Add(localizer["invalid_user_login"], Severity.Error);
            errorMessage = localizer["invalid_user_login"];
            return;
        }
        
        var cardProfileResponse = await Http.GetFromJsonAsync<BareboneCardProfile>($"ui/card/getBy/{AccessCode}");
        cardProfileResponse.ThrowIfNull();

        _cardProfile = cardProfileResponse;

        if (_cardProfile.CardId == 0)
        {
            Snackbar.Add(localizer["invalid_user_login"], Severity.Error);
            errorMessage = localizer["invalid_user_login"];
        }
    }
    
    private void OnEditCardClicked()
    {
        NavManager.NavigateTo($"Cards/Customize/{ChipId}"); 
    }
    
    private void OnViewCardClicked()
    {
        NavManager.NavigateTo($"Cards/ViewDetail/{ChipId}"); 
    }
}