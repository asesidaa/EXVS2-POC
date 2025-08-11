using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Services.Preview;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Triad;

public class TriadDataService : ITriadDataService
{
    private readonly HttpClient _client;
    private readonly IGeneralPreviewService _generalPreviewService;
    private readonly ILogger<ITriadDataService> _logger;
    
    private Dictionary<uint, IdValuePair> triadSkills = new();
    private Dictionary<uint, GeneralPreview> triadTeamBanners = new();
    
    private List<IdValuePair> sortedTriadSkillList = new();
    private List<GeneralPreview> sortedTeamBannerList = new();
    
    public TriadDataService(HttpClient client, IGeneralPreviewService generalPreviewService, ILogger<ITriadDataService> logger)
    {
        _client = client;
        _generalPreviewService = generalPreviewService;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var triadSkillList = await _client.GetFromJsonAsync<List<IdValuePair>>("data/triad/TriadSkills.json");
        triadSkillList.ThrowIfNull();
        triadSkills = triadSkillList.ToDictionary(ms => ms.Id);
        sortedTriadSkillList = triadSkillList.OrderBy(title => title.Id).ToList();
        
        var triadTeamBannerList = await _client.GetFromJsonAsync<List<GeneralPreview>>("data/triad/TriadTeamBanners.json");
        triadTeamBannerList.ThrowIfNull();
        triadTeamBanners = _generalPreviewService.CreateGeneralPreviewDictionary(triadTeamBannerList);
        sortedTeamBannerList = _generalPreviewService.CreateSortedGeneralPreviewList(triadTeamBannerList);
    }
    
    public IReadOnlyList<IdValuePair> GetSortedTriadSkillList()
    {
        return sortedTriadSkillList;
    }
    public IdValuePair? GetTriadSkill(uint id)
    {
        return triadSkills.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<GeneralPreview> GetTriadTeamBackgroundSortedById()
    {
        return sortedTeamBannerList;
    }
    
    public GeneralPreview? GetTriadTeamBackgroundById(uint id)
    {
        return triadTeamBanners.GetValueOrDefault(id);
    }
}