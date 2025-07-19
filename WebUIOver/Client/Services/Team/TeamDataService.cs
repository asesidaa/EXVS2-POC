using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Services.Preview;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Team;

public class TeamDataService : ITeamDataService
{
    private readonly HttpClient _client;
    private readonly IGeneralPreviewService _generalPreviewService;
    private readonly ILogger<TeamDataService> _logger;
    
    private Dictionary<uint, GeneralPreview> teamNameFontColors = new();
    private Dictionary<uint, GeneralPreview> teamBackgrounds = new();
    private Dictionary<uint, GeneralPreview> teamEffects = new();
    private Dictionary<uint, GeneralPreview> teamEmblems = new();
    private Dictionary<uint, TagTeamMastery> tagTeamMasteries = new();
    
    private List<GeneralPreview> sortedTeamNameFontColorList = new();
    private List<GeneralPreview> sortedTeamBackgroundList = new();
    private List<GeneralPreview> sortedTeamEffectList = new();
    private List<GeneralPreview> sortedTeamEmblemList = new();
    private List<TagTeamMastery> sortedTagTeamMasteryList = new();

    public TeamDataService(HttpClient client, IGeneralPreviewService generalPreviewService, ILogger<TeamDataService> logger)
    {
        _client = client;
        _generalPreviewService = generalPreviewService;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var teamBackgroundList = await _client.GetFromJsonAsync<List<GeneralPreview>>("data/team/Backgrounds.json");
        teamBackgroundList.ThrowIfNull();
        teamBackgrounds = _generalPreviewService.CreateGeneralPreviewDictionary(teamBackgroundList);
        sortedTeamBackgroundList = _generalPreviewService.CreateSortedGeneralPreviewList(teamBackgroundList);
        
        var teamEffectList = await _client.GetFromJsonAsync<List<GeneralPreview>>("data/team/Effects.json");
        teamEffectList.ThrowIfNull();
        teamEffects = _generalPreviewService.CreateGeneralPreviewDictionary(teamEffectList);
        sortedTeamEffectList = _generalPreviewService.CreateSortedGeneralPreviewList(teamEffectList);
        
        var teamEmblemList = await _client.GetFromJsonAsync<List<GeneralPreview>>("data/team/Emblems.json");
        teamEmblemList.ThrowIfNull();
        teamEmblems = _generalPreviewService.CreateGeneralPreviewDictionary(teamEmblemList);
        sortedTeamEmblemList = _generalPreviewService.CreateSortedGeneralPreviewList(teamEmblemList);
        
        var teamFontColorList = await _client.GetFromJsonAsync<List<GeneralPreview>>("data/team/NameFontColors.json");
        teamFontColorList.ThrowIfNull();
        teamNameFontColors = _generalPreviewService.CreateGeneralPreviewDictionary(teamFontColorList);
        sortedTeamNameFontColorList = _generalPreviewService.CreateSortedGeneralPreviewList(teamFontColorList);
        
        var tagTeamMasteryList = await _client.GetFromJsonAsync<List<TagTeamMastery>>("data/team/Mastery.json");
        tagTeamMasteryList.ThrowIfNull();
        tagTeamMasteries = tagTeamMasteryList.ToDictionary(mastery => mastery.Id);
        sortedTagTeamMasteryList = tagTeamMasteryList.OrderBy(mastery => mastery.Id).ToList();
    }

    public GeneralPreview? GetTeamNameFontColorById(uint id)
    {
        return teamNameFontColors.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<GeneralPreview> GetTeamNameFontColorsSortedById()
    {
        return sortedTeamNameFontColorList;
    }
    
    public GeneralPreview? GetTeamEffectById(uint id)
    {
        return teamEffects.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<GeneralPreview> GetTeamEffectsSortedById()
    {
        return sortedTeamEffectList;
    }
    
    public GeneralPreview? GetTeamEmblemById(uint id)
    {
        return teamEmblems.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<GeneralPreview> GetTeamEmblemsSortedById()
    {
        return sortedTeamEmblemList;
    }
    public GeneralPreview? GetTeamBackgroundById(uint id)
    {
        return teamBackgrounds.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<GeneralPreview> GetTeamBackgroundsSortedById()
    {
        return sortedTeamBackgroundList;
    }
    
    public TagTeamMastery? GetTagTeamMasteryById(uint id)
    {
        return tagTeamMasteries.GetValueOrDefault(id);
    }
    
    public IReadOnlyList<TagTeamMastery> GetTagTeamMasterySortedById()
    {
        return sortedTagTeamMasteryList;
    }
}