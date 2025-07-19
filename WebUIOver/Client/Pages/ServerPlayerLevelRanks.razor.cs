using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Throw;
using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Rank;

namespace WebUIOver.Client.Pages;

public partial class ServerPlayerLevelRanks
{
    [Inject]
    private IJSRuntime? _jsRuntime { get; set; }

    private readonly List<BreadcrumbItem> breadcrumbs = new();
    
    private string? errorMessage = null;
    private bool _loading = true;
    private PlayerLevelRankData _playerLevelRankData = new();
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        breadcrumbs.Add(new BreadcrumbItem(localizer["server_player_level_ranks"], href: "/ServerPlayerLevelRanks", disabled: false));
        
        var playerLevelRankData = await Http.GetFromJsonAsync<PlayerLevelRankData>("/ui/rank/player-level-rank/getPlayerLevelRanks");
        playerLevelRankData.ThrowIfNull();

        _playerLevelRankData = playerLevelRankData;
    }
    
    protected override void OnParametersSet()
    {
        if (_playerLevelRankData == new PlayerLevelRankData())
        {
            _loading = true;
            return;
        }

        _loading = false;
    }
    
    private uint GetTargetExp(PlayerLevelRankDto playerLevelRankDto)
    {
        if (playerLevelRankDto.PrestigeId == 1)
        {
            return _playerLevelRankData.ExpRequirement.Round2Exp;
        }
        
        if (playerLevelRankDto.PrestigeId == 2)
        {
            return _playerLevelRankData.ExpRequirement.Round3Exp;
        }
        
        if (playerLevelRankDto.PrestigeId == 3)
        {
            return _playerLevelRankData.ExpRequirement.RoundExExp;
        }
        
        return _playerLevelRankData.ExpRequirement.Round1Exp;
    }
    
    private string GetPlayerLevelClass(PlayerLevelRankDto playerLevelRankDto)
    {
        if (playerLevelRankDto.PrestigeId == 1)
        {
            return "player-level-stage-two";
        }
        
        if (playerLevelRankDto.PrestigeId == 2)
        {
            return "player-level-stage-three";
        }
        
        if (playerLevelRankDto.PrestigeId == 3)
        {
            return "player-level-stage-ex";
        }
        
        return "player-level-stage-one";
    }
}