using System.Net.Http.Json;
using Throw;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Display;

public class DisplayOptionDataService : IDisplayOptionDataService
{
    private readonly HttpClient _client;
    private readonly ILogger<DisplayOptionDataService> _logger;
    
    private Dictionary<uint, IdValuePair> _displayOptions = new();
    private Dictionary<uint, IdValuePair> _playerLevelDisplayOptions = new();
    
    private List<IdValuePair> _sortedDisplayOptionList = new();
    private List<IdValuePair> _sortedPlayerLevelDisplayOptionList = new();
    
    public DisplayOptionDataService(HttpClient client, ILogger<DisplayOptionDataService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var displayOptionList = await _client.GetFromJsonAsync<List<IdValuePair>>("data/display/DisplayOptions.json");
        displayOptionList.ThrowIfNull();
        _displayOptions = displayOptionList.ToDictionary(pair => pair.Id);
        _sortedDisplayOptionList = displayOptionList.OrderBy(title => title.Id).ToList();
        
        var playerLevelDisplayOptionList = await _client.GetFromJsonAsync<List<IdValuePair>>("data/display/PlayerLevelDisplayOptions.json");
        playerLevelDisplayOptionList.ThrowIfNull();
        _playerLevelDisplayOptions = playerLevelDisplayOptionList.ToDictionary(pair => pair.Id);
        _sortedPlayerLevelDisplayOptionList = playerLevelDisplayOptionList.OrderBy(title => title.Id).ToList();
    }

    public IReadOnlyList<IdValuePair> GetDisplayOptionsSortedById()
    {
        return _sortedDisplayOptionList;
    }
    
    public IReadOnlyList<IdValuePair> GetPlayerLevelDisplayOptionsSortedById()
    {
        return _sortedPlayerLevelDisplayOptionList;
    }
}