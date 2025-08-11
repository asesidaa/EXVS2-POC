using System.Net.Http.Json;
using Throw;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Navi;

public class NaviDataService : INaviDataService
{
    private readonly HttpClient _client;
    private readonly ILogger<NaviDataService> _logger;
    
    private Dictionary<uint, Navigator> _navigator = new();
    private List<Navigator> _sortedNavigatorList = new();
    
    public NaviDataService(HttpClient client, ILogger<NaviDataService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var naviList = await _client.GetFromJsonAsync<List<Navigator>>("data/Navigators.json");
        naviList.ThrowIfNull();
        _navigator = naviList.ToDictionary(ms => ms.Id);
        _sortedNavigatorList = naviList.OrderBy(title => title.Id).ToList();
    }

    public IReadOnlyList<Navigator> GetNavigatorSortedById()
    {
        return _sortedNavigatorList;
    }
    
    public List<Navigator> GetWritableNavigatorSortedById()
    {
        return _sortedNavigatorList;
    }
    
    public Navigator? GetNavigatorById(uint id)
    {
        return _navigator.GetValueOrDefault(id);
    }
}