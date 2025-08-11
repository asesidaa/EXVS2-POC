using System.Net.Http.Json;
using Throw;
using WebUIOver.Shared.Dto.Triad;

namespace WebUIOver.Client.Services.Triad;

public class TriadStageDataService : ITriadStageDataService
{
    private readonly HttpClient _client;
    private readonly ILogger<TriadStageDataService> _logger;
    
    
    private List<TriadCourseConfig> triadCourses = new();
    
    public TriadStageDataService(HttpClient client, ILogger<TriadStageDataService> logger)
    {
        _client = client;
        _logger = logger;
    }
    
    public async Task InitializeAsync()
    {
        var triadCourseList = await _client.GetFromJsonAsync<List<TriadCourseConfig>>("data/triad/TriadStages.json");
        triadCourseList.ThrowIfNull();
        triadCourses = triadCourseList;
    }
    
    public List<TriadCourseConfig> GetTriadCourseConfigs()
    {
        return triadCourses;
    }
}