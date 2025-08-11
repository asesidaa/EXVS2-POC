using System.Net.Http.Json;
using Throw;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Mastery;

public class FamiliarityDataService : IFamiliarityDataService
{
    private readonly HttpClient _client;
    private readonly ILogger<FamiliarityDataService> _logger;
    
    private Dictionary<uint, Familiarity> _naviFamiliarities = new();
    private Dictionary<uint, Familiarity> _msFamiliarities = new();
    private List<Familiarity> _sortedNaviFamiliarityList = new();
    private List<Familiarity> _sortedMsFamiliarityList = new();

    public FamiliarityDataService(HttpClient client, ILogger<FamiliarityDataService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var naviFamiliarityList = await _client.GetFromJsonAsync<List<Familiarity>>("data/mastery/NaviFamiliarity.json");
        naviFamiliarityList.ThrowIfNull();
        _naviFamiliarities = naviFamiliarityList.ToDictionary(naviFamiliarity => naviFamiliarity.Id);
        _sortedNaviFamiliarityList = naviFamiliarityList.OrderBy(naviFamiliarity => naviFamiliarity.Id).ToList();
        
        var msFamiliarityList = await _client.GetFromJsonAsync<List<Familiarity>>("data/mastery/MobileSuitFamiliarity.json");
        msFamiliarityList.ThrowIfNull();
        _msFamiliarities = msFamiliarityList.ToDictionary(msFamiliarity => msFamiliarity.Id);
        _sortedMsFamiliarityList = msFamiliarityList.OrderBy(msFamiliarity => msFamiliarity.Id).ToList();
    }
    
    public IReadOnlyList<Familiarity> GetNaviFamiliaritySortedById()
    {
        return _sortedNaviFamiliarityList;
    }
    
    public IReadOnlyList<Familiarity> GetMsFamiliaritySortedById()
    {
        return _sortedMsFamiliarityList;
    }
}