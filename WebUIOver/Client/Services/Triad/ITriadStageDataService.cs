using WebUIOver.Shared.Dto.Triad;

namespace WebUIOver.Client.Services.Triad;

public interface ITriadStageDataService
{
    public Task InitializeAsync();
    public List<TriadCourseConfig> GetTriadCourseConfigs();
}