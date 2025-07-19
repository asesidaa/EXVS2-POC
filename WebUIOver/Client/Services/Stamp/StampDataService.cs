using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Services.Preview;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Stamp;

public class StampDataService : IStampDataService
{
    private readonly HttpClient _client;
    private readonly IGeneralPreviewService _generalPreviewService;
    private readonly ILogger<StampDataService> _logger;
    
    private Dictionary<uint, GeneralPreview> _stamps = new();
    private List<GeneralPreview> _sortedStampsList = new();

    public StampDataService(HttpClient client, IGeneralPreviewService generalPreviewService, ILogger<StampDataService> logger)
    {
        _client = client;
        _generalPreviewService = generalPreviewService;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var stampList = await _client.GetFromJsonAsync<List<GeneralPreview>>("data/messages/Stamps.json");
        stampList.ThrowIfNull();
        _stamps = _generalPreviewService.CreateGeneralPreviewDictionary(stampList);
        _sortedStampsList = _generalPreviewService.CreateSortedGeneralPreviewList(stampList);
    }

    public GeneralPreview? GetStampById(uint id)
    {
        return _stamps.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<GeneralPreview> GetStampsSortedById()
    {
        return _sortedStampsList;
    }
}