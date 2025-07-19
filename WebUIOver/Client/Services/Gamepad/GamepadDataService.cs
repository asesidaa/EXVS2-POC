using System.Net.Http.Json;
using Throw;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Gamepad;

public class GamepadDataService : IGamepadDataService
{
    private readonly HttpClient _client;
    private readonly ILogger<GamepadDataService> _logger;
    
    private Dictionary<uint, IdValuePair> gamepadOptions = new();
    private List<IdValuePair> sortedGamepadOptionList = new();

    public GamepadDataService(HttpClient client, ILogger<GamepadDataService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var gamepadOptionList = await _client.GetFromJsonAsync<List<IdValuePair>>("data/GamepadOptions.json");
        gamepadOptionList.ThrowIfNull();
        gamepadOptions = gamepadOptionList.ToDictionary(gamepadOption => gamepadOption.Id);
        sortedGamepadOptionList = gamepadOptionList.OrderBy(gamepadOption => gamepadOption.Id).ToList();
    }
    
    public IReadOnlyList<IdValuePair> GetSortedGamepadOptionList()
    {
        return sortedGamepadOptionList;
    }
}