using System.Net.Http.Json;
using Throw;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.MS;

public class MobileSuitDataService : IMobileSuitDataService
{
    private readonly HttpClient _client;
    private readonly ILogger<IMobileSuitDataService> _logger;
    
    private Dictionary<uint, MobileSuit> _mobileSuits = new();
    private List<MobileSuit> _sortedMobileSuitList = new();

    public MobileSuitDataService(HttpClient client, ILogger<IMobileSuitDataService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var msList = await _client.GetFromJsonAsync<List<MobileSuit>>("data/MobileSuits.json");
        msList.ThrowIfNull();
        _mobileSuits = msList.ToDictionary(ms => ms.Id);
        _sortedMobileSuitList = msList.OrderBy(title => title.Id).ToList();
    }
    
    public MobileSuit? GetMobileSuitById(uint id)
    {
        return _mobileSuits.GetValueOrDefault(id);
    }

    public IReadOnlyList<MobileSuit> GetMobileSuitSortedById()
    {
        return _sortedMobileSuitList;
    }
    
    public List<MobileSuit> GetWritableMobileSuitSortedById()
    {
        return _sortedMobileSuitList;
    }
}