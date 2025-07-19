using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Services.Preview;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Sticker;

public class StickerService : IStickerService
{
    private readonly HttpClient _client;
    private readonly IGeneralPreviewService _generalPreviewService;
    private readonly ILogger<IStickerService> _logger;
    
    private Dictionary<uint, TrackerType> _trackerTypes = new();
    private Dictionary<uint, GeneralPreview> _stickerBackgrounds = new();
    private Dictionary<uint, GeneralPreview> _stickerEffects = new();
    private List<TrackerType> _sortedTrackerTypes = new();
    private List<GeneralPreview> _sortedStickerBackgrounds = new();
    private List<GeneralPreview> _sortedStickerEffects = new();

    public StickerService(HttpClient client, IGeneralPreviewService generalPreviewService, ILogger<IStickerService> logger)
    {
        _client = client;
        _generalPreviewService = generalPreviewService;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var trackerTypeList = await _client.GetFromJsonAsync<List<TrackerType>>("data/sticker/TrackerTypes.json");
        trackerTypeList.ThrowIfNull();
        _trackerTypes = trackerTypeList.ToDictionary(trackerType => trackerType.Id);
        _sortedTrackerTypes = trackerTypeList.OrderBy(trackerType => trackerType.Id).ToList();
        
        var stickerBackgroundList = await _client.GetFromJsonAsync<List<GeneralPreview>>("data/sticker/Backgrounds.json");
        stickerBackgroundList.ThrowIfNull();
        _stickerBackgrounds = _generalPreviewService.CreateGeneralPreviewDictionary(stickerBackgroundList);
        _sortedStickerBackgrounds = _generalPreviewService.CreateSortedGeneralPreviewList(stickerBackgroundList);
        
        var stickerEffectList = await _client.GetFromJsonAsync<List<GeneralPreview>>("data/sticker/Effects.json");
        stickerEffectList.ThrowIfNull();
        _stickerEffects = _generalPreviewService.CreateGeneralPreviewDictionary(stickerEffectList);
        _sortedStickerEffects = _generalPreviewService.CreateSortedGeneralPreviewList(stickerEffectList);
    }
    
    public TrackerType? GetTrackerTypeById(uint id)
    {
        return _trackerTypes.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<TrackerType> GetTrackerTypesSortedById()
    {
        return _sortedTrackerTypes;
    }
    
    public GeneralPreview? GetStickerBackgroundById(uint id)
    {
        return _stickerBackgrounds.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<GeneralPreview> GetStickerBackgroundsSortedById()
    {
        return _sortedStickerBackgrounds;
    }
    
    public GeneralPreview? GetStickerEffectById(uint id)
    {
        return _stickerEffects.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<GeneralPreview> GetStickerEffectsSortedById()
    {
        return _sortedStickerEffects;
    }
}