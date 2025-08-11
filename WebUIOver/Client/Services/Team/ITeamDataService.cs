using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Team;

public interface ITeamDataService
{
    public Task InitializeAsync();
    public GeneralPreview? GetTeamNameFontColorById(uint id);
    public IReadOnlyList<GeneralPreview> GetTeamNameFontColorsSortedById();
    public GeneralPreview? GetTeamEffectById(uint id);
    public IReadOnlyList<GeneralPreview> GetTeamEffectsSortedById();
    public GeneralPreview? GetTeamEmblemById(uint id);
    public IReadOnlyList<GeneralPreview> GetTeamEmblemsSortedById();
    public GeneralPreview? GetTeamBackgroundById(uint id);
    public IReadOnlyList<GeneralPreview> GetTeamBackgroundsSortedById();
    public TagTeamMastery? GetTagTeamMasteryById(uint id);
    public IReadOnlyList<TagTeamMastery> GetTagTeamMasterySortedById();
}