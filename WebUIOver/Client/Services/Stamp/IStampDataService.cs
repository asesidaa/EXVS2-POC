using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Stamp;

public interface IStampDataService
{
    public Task InitializeAsync();
    public GeneralPreview? GetStampById(uint id);
    public IReadOnlyList<GeneralPreview> GetStampsSortedById();
}