using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Services.Preview;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Common;

public class CommonDataService : ICommonDataService
{
    private readonly HttpClient _client;
    private readonly IGeneralPreviewService _generalPreviewService;
    private readonly ILogger<CommonDataService> _logger;
    
    private Dictionary<uint, Bgm> _bgm = new();
    private Dictionary<uint, GeneralPreview> _gauge = new();
    private Dictionary<uint, GeneralPreview> _stage = new();
    
    private List<Bgm> _sortedBgmList = new();
    private List<GeneralPreview> _sortedGaugeList = new();
    private List<GeneralPreview> _sortedStageList = new();
    
    public CommonDataService(HttpClient client, IGeneralPreviewService generalPreviewService, ILogger<CommonDataService> logger)
    {
        _client = client;
        _generalPreviewService = generalPreviewService;
        _logger = logger;
    }
    
    public async Task InitializeAsync()
    {
        var bgmList = await _client.GetFromJsonAsync<List<Bgm>>("data/Bgms.json");
        bgmList.ThrowIfNull();
        _bgm = bgmList.ToDictionary(bgm => bgm.Id);
        _sortedBgmList = bgmList.OrderBy(bgm => bgm.Id).ToList();
        
        var gaugeList = await _client.GetFromJsonAsync<List<GeneralPreview>>("data/Gauges.json");
        gaugeList.ThrowIfNull();
        _gauge = _generalPreviewService.CreateGeneralPreviewDictionary(gaugeList);
        _sortedGaugeList = _generalPreviewService.CreateSortedGeneralPreviewList(gaugeList);
        
        var stageList = await _client.GetFromJsonAsync<List<GeneralPreview>>("data/Stages.json");
        stageList.ThrowIfNull();
        _stage = _generalPreviewService.CreateGeneralPreviewDictionary(stageList);
        _sortedStageList = _generalPreviewService.CreateSortedGeneralPreviewList(stageList);
    }
    
    public IReadOnlyList<Bgm> GetBgmSortedById()
    {
        return _sortedBgmList;
    }

    public Bgm? GetBgmById(uint id)
    {
        return _bgm.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<GeneralPreview> GetGaugesSortedById()
    {
        return _sortedGaugeList;
    }

    public GeneralPreview? GetGaugeById(uint id)
    {
        return _gauge.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<GeneralPreview> GetStagesSortedById()
    {
        return _sortedStageList;
    }

    public GeneralPreview? GetStageById(uint id)
    {
        return _stage.GetValueOrDefault(id);
    }
}