using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Mastery;

public interface IFamiliarityDataService
{
    public Task InitializeAsync();
    public IReadOnlyList<Familiarity> GetNaviFamiliaritySortedById();
    public IReadOnlyList<Familiarity> GetMsFamiliaritySortedById();
}