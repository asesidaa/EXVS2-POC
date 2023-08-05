using System.Net.Http.Json;
using Throw;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Json;

namespace WebUI.Client.Services;

public class DataService : IDataService
{
    private Dictionary<uint, MobileSuit> mobileSuits = new();
    private Dictionary<uint, Bgm> bgm = new();
    private Dictionary<uint, Gauge> gauge = new();
    private Dictionary<uint, Navigator> navigator = new();
    private Dictionary<uint, IdValuePair> triadSkill = new();
    private Dictionary<uint, IdValuePair> triadTeamBanner = new();

    private List<MobileSuit> sortedMobileSuitList = new();
    private List<Bgm> sortedBgmList = new();
    private List<Gauge> sortedGaugeList = new();
    private List<Navigator> sortedNavigatorList = new();
    private List<IdValuePair> sortedTriadSkillList = new();
    private List<IdValuePair> sortedTriadTeamBannerList = new();

    private readonly HttpClient client;
    private readonly ILogger<DataService> logger;

    public DataService(HttpClient client, ILogger<DataService> logger)
    {
        this.client = client;
        this.logger = logger;
    }

    public async Task InitializeAsync()
    {
        var msList = await client.GetFromJsonAsync<List<MobileSuit>>("data/MobileSuits.json?v=2");
        msList.ThrowIfNull();
        mobileSuits = msList.ToDictionary(ms => ms.Id);
        sortedMobileSuitList = msList.OrderBy(title => title.Id).ToList();

        var bgmList = await client.GetFromJsonAsync<List<Bgm>>("data/Bgms.json");
        bgmList.ThrowIfNull();
        bgm = bgmList.ToDictionary(ms => ms.Id);
        sortedBgmList = bgmList.OrderBy(title => title.Id).ToList();

        var gaugeList = await client.GetFromJsonAsync<List<Gauge>>("data/Gauges.json");
        gaugeList.ThrowIfNull();
        gauge = gaugeList.ToDictionary(ms => ms.Id);
        sortedGaugeList = gaugeList.OrderBy(title => title.Id).ToList();

        var naviList = await client.GetFromJsonAsync<List<Navigator>>("data/Navigators.json?v=2");
        naviList.ThrowIfNull();
        navigator = naviList.ToDictionary(ms => ms.Id);
        sortedNavigatorList = naviList.OrderBy(title => title.Id).ToList();
        
        var triadSkillList = await client.GetFromJsonAsync<List<IdValuePair>>("data/TriadSkills.json");
        triadSkillList.ThrowIfNull();
        triadSkill = triadSkillList.ToDictionary(ms => ms.Id);
        sortedTriadSkillList = triadSkillList.OrderBy(title => title.Id).ToList();
        
        var triadTeamBannerList = await client.GetFromJsonAsync<List<IdValuePair>>("data/TriadTeamBanners.json");
        triadTeamBannerList.ThrowIfNull();
        triadTeamBanner = triadTeamBannerList.ToDictionary(ms => ms.Id);
        sortedTriadTeamBannerList = triadTeamBannerList.OrderBy(title => title.Id).ToList();
    }

    public IReadOnlyList<MobileSuit> GetMobileSuitSortedById()
    {
        return sortedMobileSuitList;
    }
    
    public IReadOnlyList<MobileSuit> GetCostumeMobileSuitSortedById()
    {
        return sortedMobileSuitList.FindAll(ms => ms.Costumes != null && ms.Costumes.Count > 1);
    }

    public MobileSuit? GetMobileSuitById(uint id)
    {
        return mobileSuits.GetValueOrDefault(id);
    }

    public IReadOnlyList<Bgm> GetBgmSortedById()
    {
        return sortedBgmList;
    }

    public Bgm? GetBgmById(uint id)
    {
        return bgm.GetValueOrDefault(id);
    }

    public IReadOnlyList<Gauge> GetGaugeSortedById()
    {
        return sortedGaugeList;
    }

    public Gauge? GetGaugeById(uint id)
    {
        return gauge.GetValueOrDefault(id);
    }

    public IReadOnlyList<Navigator> GetNavigatorSortedById()
    {
        return sortedNavigatorList;
    }
    public Navigator? GetNavigatorById(uint id)
    {
        return navigator.GetValueOrDefault(id);
    }
    public IReadOnlyList<IdValuePair> GetSortedTriadSkillList()
    {
        return sortedTriadSkillList;
    }
    public IdValuePair? GetTriadSkill(uint id)
    {
        return triadSkill.GetValueOrDefault(id);
    }
    public IReadOnlyList<IdValuePair> GetSortedTriadTeamBannerList()
    {
        return sortedTriadTeamBannerList;
    }
    public IdValuePair? GetTriadTeamBanner(uint id)
    {
        return triadTeamBanner.GetValueOrDefault(id);
    }
}