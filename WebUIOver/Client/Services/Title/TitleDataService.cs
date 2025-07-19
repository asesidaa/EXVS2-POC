using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Services.Preview;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Title;

public class TitleDataService : ITitleDataService
{
    private readonly HttpClient _client;
    private readonly IGeneralPreviewService _generalPreviewService;
    private readonly ILogger<TitleDataService> _logger;
    
    private Dictionary<uint, GeneralPreview> _titles = new();
    private Dictionary<uint, GeneralPreview> _backgrounds = new();
    private Dictionary<uint, GeneralPreview> _effects = new();
    private Dictionary<uint, GeneralPreview> _ornaments = new();
    
    private List<GeneralPreview> _sortedTitleList = new();
    private List<GeneralPreview> _sortedBackgroundList = new();
    private List<GeneralPreview> _sortedEffectList = new();
    private List<GeneralPreview> _sortedOrnamentList = new();

    public TitleDataService(HttpClient client, IGeneralPreviewService generalPreviewService, ILogger<TitleDataService> logger)
    {
        _client = client;
        _generalPreviewService = generalPreviewService;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var titleList = await _client.GetFromJsonAsync<List<GeneralPreview>>("data/titles/Titles.json");
        titleList.ThrowIfNull();
        _titles = _generalPreviewService.CreateGeneralPreviewDictionary(titleList);
        _sortedTitleList = _generalPreviewService.CreateSortedGeneralPreviewList(titleList);
        
        var backgroundList = await _client.GetFromJsonAsync<List<GeneralPreview>>("data/titles/Backgrounds.json");
        backgroundList.ThrowIfNull();
        _backgrounds = _generalPreviewService.CreateGeneralPreviewDictionary(backgroundList);
        _sortedBackgroundList = _generalPreviewService.CreateSortedGeneralPreviewList(backgroundList);
        
        var effectList = await _client.GetFromJsonAsync<List<GeneralPreview>>("data/titles/Effects.json");
        effectList.ThrowIfNull();
        _effects = _generalPreviewService.CreateGeneralPreviewDictionary(effectList);
        _sortedEffectList = _generalPreviewService.CreateSortedGeneralPreviewList(effectList);
        
        var ornamentList = await _client.GetFromJsonAsync<List<GeneralPreview>>("data/titles/Ornaments.json");
        ornamentList.ThrowIfNull();
        _ornaments = _generalPreviewService.CreateGeneralPreviewDictionary(ornamentList);
        _sortedOrnamentList = _generalPreviewService.CreateSortedGeneralPreviewList(ornamentList);
    }
    
    public GeneralPreview? GetTitleById(uint id)
    {
        return _titles.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<GeneralPreview> GetTitlesSortedById()
    {
        return _sortedTitleList;
    }
    
    public GeneralPreview? GetBackgroundById(uint id)
    {
        return _backgrounds.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<GeneralPreview> GetBackgroundsSortedById()
    {
        return _sortedBackgroundList;
    }
    
    public GeneralPreview? GetEffectById(uint id)
    {
        return _effects.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<GeneralPreview> GetEffectsSortedById()
    {
        return _sortedEffectList;
    }
    
    public GeneralPreview? GetOrnamentById(uint id)
    {
        return _ornaments.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<GeneralPreview> GetOrnamentsSortedById()
    {
        return _sortedOrnamentList;
    }
}