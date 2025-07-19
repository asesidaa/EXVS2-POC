using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Title;

public interface ITitleDataService
{
    public Task InitializeAsync();
    public GeneralPreview? GetTitleById(uint id);
    public IReadOnlyList<GeneralPreview> GetTitlesSortedById();
    public GeneralPreview? GetBackgroundById(uint id);
    public IReadOnlyList<GeneralPreview> GetBackgroundsSortedById();
    public GeneralPreview? GetEffectById(uint id);
    public IReadOnlyList<GeneralPreview> GetEffectsSortedById();
    public GeneralPreview? GetOrnamentById(uint id);
    public IReadOnlyList<GeneralPreview> GetOrnamentsSortedById();
}