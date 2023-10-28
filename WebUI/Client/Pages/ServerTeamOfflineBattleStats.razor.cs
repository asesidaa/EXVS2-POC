using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Throw;
using WebUI.Client.Context;
using WebUI.Shared.Context;

namespace WebUI.Client.Pages;

public partial class ServerTeamOfflineBattleStats
{
    [Inject]
    private IJSRuntime? _jsRuntime { get; set; }

    private ServerBattlePageContext _battlePageContext { get; set; } = new();
    
    private string? errorMessage = null;
    
    private readonly List<BreadcrumbItem> breadcrumbs = new();
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        breadcrumbs.Add(new BreadcrumbItem(localizer["server_team_offline_battle_stats"], href: "/ServerTeamOfflineBattleStats", disabled: false));
        
        var constructor = new ServerBattlePageContextConstructor(Http, DataService, "Team");
        ServerBattlePageContext constructedContext = await constructor.Construct();
        constructedContext.ThrowIfNull();
        _battlePageContext = constructedContext;
    }
}