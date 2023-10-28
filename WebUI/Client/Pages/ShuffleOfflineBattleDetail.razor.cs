using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Throw;
using WebUI.Client.Context;
using WebUI.Shared.Context;

namespace WebUI.Client.Pages;

public partial class ShuffleOfflineBattleDetail
{
    [Parameter]
    public string ChipId { get; set; } = string.Empty;
    
    [Parameter]
    public string AccessCode { get; set; } = string.Empty;

    [Inject]
    private IJSRuntime? _jsRuntime { get; set; }

    private BattlePageContext _battlePageContext { get; set; } = new();
    
    private string? errorMessage = null;
    
    private readonly List<BreadcrumbItem> breadcrumbs = new()
    {
        new BreadcrumbItem("Cards", href: "/Cards"),
    };
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        breadcrumbs.Add(new BreadcrumbItem($"Card: {ChipId}", href: null, disabled: true));
        breadcrumbs.Add(new BreadcrumbItem(localizer["cardviewdetail"], href: $"/Cards/ShuffleOfflineBattleDetail/{ChipId}", disabled: false));

        AccessCode = await _jsRuntime.InvokeAsync<string>("accessCode.get");

        var constructor = new OfflineBattlePageContextConstructor(Http, DataService, AccessCode, ChipId, "Shuffle");
        BattlePageContext constructedContext = await constructor.Construct();
        constructedContext.ThrowIfNull();
        _battlePageContext = constructedContext;
    }
}