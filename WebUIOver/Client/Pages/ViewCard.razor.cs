using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using WebUIOver.Client.Command.ViewCard.Filler;
using WebUIOver.Client.Context.ViewCard;

namespace WebUIOver.Client.Pages;

public partial class ViewCard
{
    [Parameter]
    public string ChipId { get; set; } = string.Empty;
    
    [Parameter]
    public string AccessCode { get; set; } = string.Empty;

    [Inject]
    private IJSRuntime? _jsRuntime { get; set; }
    
    private string? errorMessage = null;
    
    private readonly List<BreadcrumbItem> breadcrumbs = new();
    
    private readonly ViewCardContext _viewCardContext = new();
    
    [Inject] private TriadCourseOverallResultFiller _triadCourseOverallResultFiller { get; set; }
    [Inject] private BattleHistoriesFiller _battleHistoriesFiller { get; set; }
    
    private bool _loading = true;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        breadcrumbs.Add(new BreadcrumbItem("Menu", href: $"Cards/SingularCardMenu/{ChipId}", disabled: false));

        AccessCode = await _jsRuntime.InvokeAsync<string>("accessCode.get");
        
        if (string.IsNullOrEmpty(AccessCode))
        {
            Snackbar.Add($"Invalid access code!", Severity.Error);
            return;
        }
        
        _viewCardContext.AccessCode = AccessCode;
        _viewCardContext.ChipId = ChipId;

        await _triadCourseOverallResultFiller.Fill(_viewCardContext);
        await _battleHistoriesFiller.Fill(_viewCardContext);
    }
    
    protected override void OnParametersSet()
    {
        _loading = false;
    }

}