using System.Net.Http.Json;
using Throw;
using WebUI.Shared.Dto.Common;

namespace WebUI.Client.Services;

public class DataService : IDataService
{
    private Dictionary<uint, IdValuePair> displayOptions= new();
    private Dictionary<uint, IdValuePair> echelonDisplayOptions = new();
    private Dictionary<uint, MobileSuit> mobileSuits = new();
    private Dictionary<uint, IdValuePair> bgm = new();
    private Dictionary<uint, IdValuePair> gauge = new();
    private Dictionary<uint, Navigator> navigator = new();
    private Dictionary<uint, IdValuePair> triadSkill = new();
    private Dictionary<uint, IdValuePair> triadTeamBanner = new();
    private Dictionary<uint, IdValuePair> gamepadOptions = new();

    private List<IdValuePair> sortedDisplayOptionList = new();
    private List<IdValuePair> sortedEchelonDisplayOptionList = new();
    private List<MobileSuit> sortedMobileSuitList = new();
    private List<IdValuePair> sortedBgmList = new();
    private List<IdValuePair> sortedGaugeList = new();
    private List<Navigator> sortedNavigatorList = new();
    private List<IdValuePair> sortedTriadSkillList = new();
    private List<IdValuePair> sortedTriadTeamBannerList = new();
    private List<IdValuePair> sortedGamepadOptionList = new();
    private List<TriadCourseConfig> triadCourseConfigList = new();

    private readonly HttpClient client;
    private readonly ILogger<DataService> logger;

    public DataService(HttpClient client, ILogger<DataService> logger)
    {
        this.client = client;
        this.logger = logger;
    }

    public async Task InitializeAsync()
    {
        var displayOptionList = await client.GetFromJsonAsync<List<IdValuePair>>("data/DisplayOptions.json");
        displayOptionList.ThrowIfNull();
        displayOptions = displayOptionList.ToDictionary(pair => pair.Id);
        sortedDisplayOptionList = displayOptionList.OrderBy(title => title.Id).ToList();
        
        var echelonDisplayOptionList = await client.GetFromJsonAsync<List<IdValuePair>>("data/EchelonDisplayOptions.json");
        echelonDisplayOptionList.ThrowIfNull();
        echelonDisplayOptions = echelonDisplayOptionList.ToDictionary(pair => pair.Id);
        sortedEchelonDisplayOptionList = echelonDisplayOptionList.OrderBy(title => title.Id).ToList();
        
        var msList = await client.GetFromJsonAsync<List<MobileSuit>>("data/MobileSuits.json?v=2");
        msList.ThrowIfNull();
        mobileSuits = msList.ToDictionary(ms => ms.Id);
        sortedMobileSuitList = msList.OrderBy(title => title.Id).ToList();

        var bgmList = await client.GetFromJsonAsync<List<IdValuePair>>("data/Bgms.json");
        bgmList.ThrowIfNull();
        bgm = bgmList.ToDictionary(ms => ms.Id);
        sortedBgmList = bgmList.OrderBy(title => title.Id).ToList();

        var gaugeList = await client.GetFromJsonAsync<List<IdValuePair>>("data/Gauges.json?v=2");
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
        
        var gamepadOptionList = await client.GetFromJsonAsync<List<IdValuePair>>("data/GamepadOptions.json");
        gamepadOptionList.ThrowIfNull();
        gamepadOptions = gamepadOptionList.ToDictionary(gamepadOption => gamepadOption.Id);
        sortedGamepadOptionList = gamepadOptionList.OrderBy(gamepadOption => gamepadOption.Id).ToList();
        
        var triadCourseConfigs = await client.GetFromJsonAsync<List<TriadCourseConfig>>("data/TriadCourseConfigs.json");
        triadCourseConfigs.ThrowIfNull();
        triadCourseConfigList = triadCourseConfigs;
    }

    public IReadOnlyList<IdValuePair> GetDisplayOptionsSortedById()
    {
        return sortedDisplayOptionList;
    }
    
    public IReadOnlyList<IdValuePair> GetEchelonDisplayOptionsSortedById()
    {
        return sortedEchelonDisplayOptionList;
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

    public IReadOnlyList<IdValuePair> GetBgmSortedById()
    {
        return sortedBgmList;
    }

    public IdValuePair? GetBgmById(uint id)
    {
        return bgm.GetValueOrDefault(id);
    }

    public IReadOnlyList<IdValuePair> GetGaugeSortedById()
    {
        return sortedGaugeList;
    }

    public IdValuePair? GetGaugeById(uint id)
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
    
    public IReadOnlyList<IdValuePair> GetSortedGamepadOptionList()
    {
        return sortedGamepadOptionList;
    }
    
    public List<TriadCourseConfig> GetTriadStageConfigs()
    {
        return triadCourseConfigList;
    }
}