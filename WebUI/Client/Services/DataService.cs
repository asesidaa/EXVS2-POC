using System.Net.Http.Json;
using Throw;
using WebUI.Shared.Dto.Json;

namespace WebUI.Services;

public class DataService : IDataService
{
    private Dictionary<uint, MobileSuit> _mobileSuits = new();
    private Dictionary<uint, Bgm> _bgm = new();
    private Dictionary<uint, Gauge> _gauge = new();
    private Dictionary<uint, Navigator> _navigator = new();

    private List<MobileSuit> _sortedMobileSuitList = new();
    private List<Bgm> _sortedBgmList = new();
    private List<Gauge> _sortedGaugeList = new();
    private List<Navigator> _sortedNavigatorList = new();

    private readonly HttpClient client;
    private readonly ILogger<DataService> _logger;

    public DataService(HttpClient client, ILogger<DataService> logger)
    {
        this.client = client;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var msList = await client.GetFromJsonAsync<List<MobileSuit>>("data/MobileSuits.json");
        msList.ThrowIfNull();
        _mobileSuits = msList.ToDictionary(ms => ms.Id);
        _sortedMobileSuitList = msList.OrderBy(title => title.Id).ToList();

        var bgmList = await client.GetFromJsonAsync<List<Bgm>>("data/Bgms.json");
        bgmList.ThrowIfNull();
        _bgm = bgmList.ToDictionary(ms => ms.Id);
        _sortedBgmList = bgmList.OrderBy(title => title.Id).ToList();

        var gaugeList = await client.GetFromJsonAsync<List<Gauge>>("data/Gauges.json");
        gaugeList.ThrowIfNull();
        _gauge = gaugeList.ToDictionary(ms => ms.Id);
        _sortedGaugeList = gaugeList.OrderBy(title => title.Id).ToList();

        var naviList = await client.GetFromJsonAsync<List<Navigator>>("data/Navigators.json");
        naviList.ThrowIfNull();
        _navigator = naviList.ToDictionary(ms => ms.Id);
        _sortedNavigatorList = naviList.OrderBy(title => title.Id).ToList();
    }

    public IReadOnlyList<MobileSuit> GetMobileSuitSortedById()
    {
        return _sortedMobileSuitList;
    }

    public MobileSuit? GetMobileSuitById(uint id)
    {
        return _mobileSuits.GetValueOrDefault(id);
    }

    public IReadOnlyList<Bgm> GetBgmSortedById()
    {
        return _sortedBgmList;
    }

    public Bgm? GetBgmById(uint id)
    {
        return _bgm.GetValueOrDefault(id);
    }

    public IReadOnlyList<Gauge> GetGaugeSortedById()
    {
        return _sortedGaugeList;
    }

    public Gauge? GetGaugeById(uint id)
    {
        return _gauge.GetValueOrDefault(id);
    }

    public IReadOnlyList<Navigator> GetNavigatorSortedById()
    {
        return _sortedNavigatorList;
    }
    public Navigator? GetNavigatorById(uint id)
    {
        return _navigator.GetValueOrDefault(id);
    }
}